#include <assert.h>
#include <VG/openvg.h>

#include "font.h"
#include "ovgwrapper.h"

struct Font_T {
   VGFont vg_font;
   FT_Face ft_face;
};

typedef struct {
   float x;
   float y;
} Vector2;


#define SEGMENTS_COUNT_MAX 2048
#define COORDS_COUNT_MAX 4096
#define POINTSIZE 256

static FT_Library lib;

void font_lib_init(){
  	assert(FT_Init_FreeType(&lib) == 0);
}

void font_lib_dispose(){
   FT_Done_FreeType(lib);
}

static VGuint segments_count;
static VGubyte segments[SEGMENTS_COUNT_MAX];
static VGuint coords_count;
static VGfloat coords[COORDS_COUNT_MAX];

static VGfloat float_from_26_6(FT_Pos x)
{
   return (VGfloat)x / 64.0f;
}

static FT_Pos _26_6_from_float(float x){
   return (FT_Pos)x * 64;
}

static void convert_outline(const FT_Vector *points, const char *tags, const short *contours, short contours_count, short points_count)
{
   segments_count = 0;
   coords_count = 0;

   int lastPointIdx = 0;
   int endPointIdx;

   Vector2 lastPoint, currentPoint, nextPoint;

   for (int currentContour = 0; currentContour < contours_count; currentContour++)
   {
      int pointCount = 1;
      endPointIdx = contours[currentContour] + 1;
      
      FT_Vector ft_last = points[lastPointIdx];
      lastPoint.x = float_from_26_6(ft_last.x);
      lastPoint.y = float_from_26_6(ft_last.y);

      // start shape
      segments[segments_count++] = VG_MOVE_TO;
      coords[coords_count++] = lastPoint.x;
      coords[coords_count++] = lastPoint.y;
      
      int visitPointIdx = lastPointIdx + 1;

      while (visitPointIdx <= endPointIdx)
      {
            int currentPointIdx = (visitPointIdx == endPointIdx) ? lastPointIdx : visitPointIdx;
            int nextPointIdx = (visitPointIdx == endPointIdx - 1) ? lastPointIdx : (visitPointIdx + 1);
            FT_Vector ft_current = points[currentPointIdx];
            currentPoint.x = float_from_26_6(ft_current.x);
            currentPoint.y = float_from_26_6(ft_current.y);
            
            // point is 'on'
            if (tags[currentPointIdx] & 1)
            {
               //line
               ++visitPointIdx;
               segments[segments_count++] = VG_LINE_TO;
               coords[coords_count++] = currentPoint.x;
               coords[coords_count++] = currentPoint.y;

               pointCount += 1;
            }
            // point is 'off' --> conic/quadratic
            else
            {   // next point is 'on' -> spline
               if (tags[nextPointIdx] & 1)
               {   //next on
                  FT_Vector ft_next = points[nextPointIdx];
                  nextPoint.x = float_from_26_6(ft_next.x);
                  nextPoint.y = float_from_26_6(ft_next.y);
                  visitPointIdx += 2;
               }
               //next point is 'off' -> use middle point
               else
               {   
                  FT_Vector ft_conv = points[nextPointIdx];
                  Vector2 conv;
                  conv.x = float_from_26_6(ft_conv.x);
                  conv.y = float_from_26_6(ft_conv.y);

                  nextPoint.x = (currentPoint.x + conv.x) * 0.5f;
                  nextPoint.y = (currentPoint.y + conv.y) * 0.5f;
                  ++visitPointIdx;
               }

               segments[segments_count++] = VG_QUAD_TO;
               coords[coords_count++] = currentPoint.x;
               coords[coords_count++] = currentPoint.y;
               coords[coords_count++] = nextPoint.x;
               coords[coords_count++] = nextPoint.y;

               pointCount += 2;
            }
            lastPoint = nextPoint;
      }
      
      // close path
      segments[segments_count++] = VG_CLOSE_PATH;

      lastPointIdx = endPointIdx;
   }

   assert(segments_count <= SEGMENTS_COUNT_MAX); /* oops... we overwrote some memory */
   assert(coords_count <= COORDS_COUNT_MAX);
}

void font_convert_glyphs(PICFont font, unsigned int char_height, unsigned int dpi_x, unsigned int dpi_y)
{
   FT_UInt glyph_index;
   FT_ULong ch;

   assert(FT_Set_Char_Size(font->ft_face, 0, char_height, dpi_x, dpi_y) == 0);

   ch = FT_Get_First_Char(font->ft_face, &glyph_index);
   uint charCount = 0;

   //printf("First char: %c, index: %i", ch, glyph_index);

   while (glyph_index != 0)
   {
      assert(FT_Load_Glyph(font->ft_face, glyph_index, FT_LOAD_DEFAULT) == 0);

      VGPath vg_path;
      FT_Outline *outline = &font->ft_face->glyph->outline;
      if (outline->n_contours != 0) {
         vg_path = vgCreatePath(VG_PATH_FORMAT_STANDARD, VG_PATH_DATATYPE_F, 1.0f, 0.0f, 0, 0, VG_PATH_CAPABILITY_APPEND_TO);
         assert(vg_path != VG_INVALID_HANDLE);

         convert_outline(outline->points, outline->tags, outline->contours, outline->n_contours, outline->n_points);
         vgAppendPathData(vg_path, segments_count, segments, coords);
      } else {
         vg_path = VG_INVALID_HANDLE;
      }

      VGfloat origin[] = { 0.0f, 0.0f };
      VGfloat escapement[] = { float_from_26_6(font->ft_face->glyph->advance.x), float_from_26_6(font->ft_face->glyph->advance.y) };
      vgSetGlyphToPath(font->vg_font, glyph_index, vg_path, VG_FALSE, origin, escapement);

      if (vg_path != VG_INVALID_HANDLE) {
         vgDestroyPath(vg_path);
      }
      ch = FT_Get_Next_Char(font->ft_face, ch, &glyph_index);
      charCount++;
   }
   printf("Loaded %i glyphs.", charCount);
}

PICFont picLoadFont(const char* file){
   PICFont font = malloc(sizeof(struct Font_T));
   
   assert(FT_New_Face(lib, file, 0, &font->ft_face) == 0);
   puts(font->ft_face->family_name);
   puts(font->ft_face->style_name);

   font->vg_font = vgCreateFont(0);

   int ptsize = POINTSIZE << 6; // freetype takes size in points, in 26.6 format.
   font_convert_glyphs(font, ptsize, 0, 0);
   puts("Glyps loaded");

   return font;
}

void picDeleteFont(PICFont font){
   if (font->ft_face)
      FT_Done_Face(font->ft_face);
   if (font->vg_font)
      vgDestroyFont(font->vg_font);

   memset(font, 0, sizeof(*font));
}

#define CHAR_COUNT_MAX 200
static VGuint glyph_indices[CHAR_COUNT_MAX];
static VGfloat adjustments_x[CHAR_COUNT_MAX];
static VGfloat adjustments_y[CHAR_COUNT_MAX];
static VGfloat matrix[9]; 

// Draws the first char_count characters from text, with adjustments, starting 
// from the current origin.  The peek argument indicates whether to peek ahead 
// and get a final adjustment based on the next character past char_count, or 
// else just assume that this is the end of the text and add no final 
// adjustment.
static void draw_chars(PICFont font, const char *text, int char_count, VGbitfield paint_modes, int peek) {
   // Put in first character
   glyph_indices[0] = FT_Get_Char_Index(font->ft_face, text[0]);
   int prev_glyph_index = glyph_indices[0];

   // Calculate glyph_indices and adjustments
   int i;
   FT_Vector kern;
   for (i = 1; i != char_count; ++i) {
      int glyph_index = FT_Get_Char_Index(font->ft_face, text[i]);
      if (!glyph_index) { return; }
      glyph_indices[i] = glyph_index;

      if (FT_Get_Kerning(font->ft_face, prev_glyph_index, glyph_index, FT_KERNING_DEFAULT, &kern)) assert(0);
      adjustments_x[i - 1] = float_from_26_6(kern.x);
      adjustments_y[i - 1] = float_from_26_6(kern.y);

      prev_glyph_index = glyph_index;
   }

   // Get the last adjustment?
   if (peek) {
      int peek_glyph_index = FT_Get_Char_Index(font->ft_face, text[i]);
      if (!peek_glyph_index) { return; }
      if (FT_Get_Kerning(font->ft_face, prev_glyph_index, peek_glyph_index, FT_KERNING_DEFAULT, &kern)) assert(0);
      adjustments_x[char_count - 1] = float_from_26_6(kern.x);
      adjustments_y[char_count - 1] = float_from_26_6(kern.y);
   } else {
      adjustments_x[char_count - 1] = 0.0f;
      adjustments_y[char_count - 1] = 0.0f;
   }

   vgDrawGlyphs(font->vg_font, char_count, glyph_indices, adjustments_x, adjustments_y, paint_modes, VG_TRUE);
}

// Goes to the x,y position and draws arbitrary number of characters, draws 
// iteratively if the char_count exceeds the max buffer size given above.
static void draw_line(PICFont font, VGfloat x, VGfloat y, const char *text, int char_count, VGbitfield paint_modes) {
   if (char_count == 0) return;

   // Set origin to requested x,y
   VGfloat glor[] = { x, y };
   vgSetfv(VG_GLYPH_ORIGIN, 2, glor);

   // Draw the characters in blocks to reuse buffer memory
   const char *curr_text = text;
   int chars_left = char_count;
   while (chars_left > CHAR_COUNT_MAX) {
      draw_chars(font, curr_text, CHAR_COUNT_MAX, paint_modes, 1);
      chars_left -= CHAR_COUNT_MAX;
      curr_text += CHAR_COUNT_MAX;
   }

   // Draw the last block
   draw_chars(font, curr_text, chars_left, paint_modes, 0);
}

void picDrawText(PICFont font, PICPoint point, const char *text, unsigned text_length, float size, uint fill, uint stroke, float lineWidth)
{
   VGbitfield fillType = 0;

   if(fill != 0){
      fillType |= VG_FILL_PATH;
      vgSetPaint(fill, VG_FILL_PATH);
   }

   if(stroke != 0){
      fillType |= VG_STROKE_PATH;
      vgSetPaint(stroke, VG_STROKE_PATH);
      vgSetf(VG_STROKE_LINE_WIDTH, lineWidth);
   }
   
   FT_Pos fontSize = _26_6_from_float(size);
   assert(FT_Set_Char_Size(font->ft_face, 0, fontSize, 0, 0) == 0);

   VGfloat descent = float_from_26_6(font->ft_face->size->metrics.descender);
   int last_draw = 0;
   int i = 0;

   //int y = -descent;   
   point.y -= descent;
      
   vgGetMatrix(matrix);
   vgSeti(VG_MATRIX_MODE, VG_MATRIX_GLYPH_USER_TO_SURFACE);
   vgLoadMatrix(matrix);
   vgTranslate(point.x, point.y);
   float scale = size / POINTSIZE; 
   vgScale(scale, scale);
   vgTranslate(-point.x, -point.y);

   for (;;) {
      int last = !text[i] || (text_length && i==text_length);

      if ((text[i] == '\n') || last)
      {
         draw_line(font, point.x, point.y, text + last_draw, i - last_draw, fillType);
         last_draw = i+1;
         point.y -= float_from_26_6(font->ft_face->size->metrics.height);
      }
      if (last)
      {
         break;
      }
      ++i;
   }

   vgSeti(VG_MATRIX_MODE, VG_MATRIX_PATH_USER_TO_SURFACE);
}

// Get text extents for a single line
// static void line_extents(VGFT_FONT_T *font, VGfloat *x, VGfloat *y, const char *text, int chars_count)
// {
//    int i;
//    int prev_glyph_index = 0;
//    if (chars_count == 0) return;

//    for (i=0; i < chars_count; i++)
//    {
//       int glyph_index = FT_Get_Char_Index(font->ft_face, text[i]);
//       if (!glyph_index) return;

//       if (i != 0)
//       {
//          FT_Vector kern;
//          if (FT_Get_Kerning(font->ft_face, prev_glyph_index, glyph_index,
//                             FT_KERNING_DEFAULT, &kern))
//          {
//             assert(0);
//          }
//          *x += float_from_26_6(kern.x);
//          *y += float_from_26_6(kern.y);
//       }
//       FT_Load_Glyph(font->ft_face, glyph_index, FT_LOAD_DEFAULT);
//       *x += float_from_26_6(font->ft_face->glyph->advance.x);

//       prev_glyph_index = glyph_index;
//    }
// }
