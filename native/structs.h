#ifndef STRUCTS_H
#define STRUCTS_H

typedef struct {
  float r;
  float g;
  float b;
  float a;
} PICColor;

typedef struct {
  float x;
  float y;
} PICPoint;

typedef PICPoint PICSize;

typedef struct {
  float position;
  PICColor color;
} PICColorStop;

typedef struct {
  PICPoint from;
  PICPoint to;
} PICLine;

typedef struct {
  PICPoint center;
  PICPoint offset;
  float radius;
} PICRadialCoord;

typedef uint PICPath;
typedef uint PICPaint;
typedef uint PICImage;
typedef void* PICHandle;

#endif
