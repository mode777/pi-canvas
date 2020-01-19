#ifndef OVGWRAPPER_H
#define OVGWRAPPER_H

#include "structs.h"

void ovg_init(PICHandle display, PICHandle surface);

extern void picDispose();
extern void picClearColor(PICColor);
extern void picClear(float, float, float, float);
extern void picPresent();

extern PICPaint picCreateColor(PICColor);
extern PICPaint picCreateLinearGradient(PICLine line, PICColorStop *stops, int n_stops, uint spreadmode);
extern PICPaint picCreateRadialGradient(PICRadialCoord radial, PICColorStop *stops, int n_stops, uint spreadmode);
extern void picDeletePaint(PICPaint);

extern PICPath picCreateElipse(float, float, float, float);
extern PICPath picCreateRect(float, float, float, float);
extern PICPath picCreatePolygon(PICPoint *points, int n);
extern PICPath picCreateLine(float, float, float, float);
extern PICPath picCreateRoundRect(float, float, float, float, float, float);
extern PICPath picCreatePath(const unsigned char* segments, int n_segments, float* data);
extern void picDrawPath(PICPath path, PICPaint fill, PICPaint stroke, float lineWidth);
extern void picDeletePath(PICPath);

extern void picTranslate(float x, float y);
extern void picRotate(float r);
extern void picShear(float x, float y);
extern void picScale(float x, float y);
extern void picIdentity();
extern void picPush();
extern void picPop();

#endif