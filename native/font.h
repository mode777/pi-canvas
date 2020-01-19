#ifndef FONT_H
#define FONT_H

#include <ft2build.h>
#include FT_FREETYPE_H

#include "structs.h"

typedef struct Font_T* PICFont;

void font_lib_init();
void font_lib_dispose();

extern PICFont picLoadFont(const char* file);
extern void picDeleteFont(PICFont font);
extern void picDrawText(PICFont font, PICPoint point, const char *text, unsigned text_length, float size, uint fill, uint stroke, float lineWidth);

#endif