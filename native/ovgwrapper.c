#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include "VG/openvg.h"
#include "VG/vgu.h"					   
#include "ovgwrapper.h"
#include <EGL/egl.h>
#include "eglstate.h"

static PICHandle gDisplay;
static PICHandle gSurface;

static char image_matrix_dirty = 0;
static char glyph_matrix_dirty = 0;

static VGfloat matrix_stack[128*9]; 
static int current_matrix = 0;

void update_matrix(VGMatrixMode mode, char * flag){
	if(*flag){
		vgGetMatrix(matrix_stack+(current_matrix*9));
		vgSeti(VG_MATRIX_MODE, mode);
		vgLoadMatrix(matrix_stack+(current_matrix*9));
		*flag = 0;
		vgSeti(VG_MATRIX_MODE, VG_MATRIX_PATH_USER_TO_SURFACE);
	}
}

void update_image_matrix(){
	update_matrix(VG_MATRIX_IMAGE_USER_TO_SURFACE, &image_matrix_dirty);
}

void update_glyph_matrix(){
	update_matrix(VG_MATRIX_GLYPH_USER_TO_SURFACE, &glyph_matrix_dirty);
}

void set_matrix_dirty(){
	image_matrix_dirty = 1;
	glyph_matrix_dirty = 1;
}

void picPush(){
	vgGetMatrix(matrix_stack+(current_matrix*9));
	current_matrix++;
}

void picPop(){
	if(current_matrix > 0){
		current_matrix--;
		vgLoadMatrix(matrix_stack+(current_matrix*9));
	}
}

void picTranslate(float x, float y) { 
	vgTranslate(x, y); 
	set_matrix_dirty();
}
void picRotate(float r) { 
	vgRotate(r); 
	set_matrix_dirty();
}
void picShear(float x, float y) {	
	vgShear(x, y); 
	set_matrix_dirty();
}
void picScale(float x, float y) { 
	vgScale(x, y); 
	set_matrix_dirty();
}
void picIdentity() { 
	vgLoadIdentity(); 
	set_matrix_dirty();
}

// init sets the system to its initial state
void ovg_init(PICHandle display, PICHandle surface) {
	gDisplay = display;
	gSurface = surface;

	// set defaults
	vgSeti(VG_STROKE_CAP_STYLE, VG_CAP_BUTT);
	vgSeti(VG_STROKE_JOIN_STYLE, VG_JOIN_MITER);
	vgSeti(VG_FILL_RULE, VG_NON_ZERO);
	vgSeti(VG_IMAGE_QUALITY, VG_IMAGE_QUALITY_NONANTIALIASED);
}

void picClearColor(PICColor color){
	vgSetfv(VG_CLEAR_COLOR, 4, (VGfloat *)&color);
}

void picClear(float x, float y, float w, float h){
  vgClear(x, y, w, h);
}

void picPresent(){
  assert(vgGetError() == VG_NO_ERROR);
	eglSwapBuffers(gDisplay, gSurface);
	assert(eglGetError() == EGL_SUCCESS);
}

PICPaint picCreateColor(PICColor color){
  VGPaint paint = vgCreatePaint();
	vgSetParameteri(paint, VG_PAINT_TYPE, VG_PAINT_TYPE_COLOR);
	vgSetParameterfv(paint, VG_PAINT_COLOR, 4, (VGfloat *)&color);
  return paint;
}

void setstops(VGPaint paint, VGfloat * stops, int n, VGColorRampSpreadMode spreadmode) {
	vgSetParameteri(paint, VG_PAINT_COLOR_RAMP_SPREAD_MODE, spreadmode);
	vgSetParameteri(paint, VG_PAINT_COLOR_RAMP_PREMULTIPLIED, VG_FALSE);
	vgSetParameterfv(paint, VG_PAINT_COLOR_RAMP_STOPS, 5 * n, stops);
}

PICPaint picCreateLinearGradient(PICLine line, PICColorStop *stops, int n_stops, uint spreadmode){
  VGPaint paint = vgCreatePaint();
	vgSetParameteri(paint, VG_PAINT_TYPE, VG_PAINT_TYPE_LINEAR_GRADIENT);
	vgSetParameterfv(paint, VG_PAINT_LINEAR_GRADIENT, 4, (VGfloat *)&line);
	setstops(paint, (VGfloat *)stops, n_stops, (VGColorRampSpreadMode)spreadmode);
	return paint;
}

PICPaint picCreatePICRadialGradient(PICRadialCoord PICRadial, PICColorStop *stops, int n_stops, uint spreadmode){
	VGPaint paint = vgCreatePaint();
	vgSetParameteri(paint, VG_PAINT_TYPE, VG_PAINT_TYPE_RADIAL_GRADIENT);
	vgSetParameterfv(paint, VG_PAINT_RADIAL_GRADIENT, 5, (VGfloat *)&PICRadial);
	setstops(paint, (VGfloat *)stops, n_stops, (VGColorRampSpreadMode)spreadmode);
	return paint;
}

void picDeletePaint(PICPaint handle){
	vgDestroyPaint(handle);
}

// newpath creates path data
// Changed capabilities as others not needed at the moment - allows possible
// driver optimisations.
VGPath newpath() {
  VGPath path = vgCreatePath(VG_PATH_FORMAT_STANDARD, VG_PATH_DATATYPE_F, 1.0f, 0.0f, 0, 0, VG_PATH_CAPABILITY_APPEND_TO);	// Other capabilities not needed
  return path;
}

PICPath picCreateElipse(float x, float y, float w, float h){
  VGPath path = newpath();
	vguEllipse(path, x, y, w, h);
  return (uint)path;
}

PICPath picCreateRect(float x, float y, float w, float h) {
	VGPath path = newpath();
	vguRect(path, x, y, w, h);
	return (uint)path;
}

PICPath picCreatePolygon(PICPoint *points, int n){
	VGPath path = newpath();
	vguPolygon(path, (VGfloat *)points, n, VG_FALSE);
	return (uint)path;
}

PICPath picCreateLine(float x1, float y1, float x2, float y2){
  VGPath path = newpath();
	vguLine(path, x1, y1, x2, y2);
	return (uint)path;
}

PICPath picCreateRoundRect(float x, float y, float w, float h, float rw, float rh){
	VGPath path = newpath();
	vguRoundRect(path, x, y, w, h, rw, rh);
	return (uint)path;
}

PICPath picCreatePath(const unsigned char* segments, int n_segments, float* data){
	VGPath path = newpath();	
	vgAppendPathData(path, n_segments, segments, data);
	return (uint)path;
}

void picDrawPath(PICPath path, PICPaint fill, PICPaint stroke, float width){
  VGbitfield fillType = 0;

  if(fill != 0){
    fillType |= VG_FILL_PATH;
  }

  if(stroke != 0){
    fillType |= VG_STROKE_PATH;
		vgSetf(VG_STROKE_LINE_WIDTH, width);
  }

  vgSetPaint(fill, VG_FILL_PATH);
  vgSetPaint(stroke, VG_STROKE_PATH);
	vgDrawPath(path, fillType);
}

void picDeletePath(PICPath path){
  vgDestroyPath(path);
}