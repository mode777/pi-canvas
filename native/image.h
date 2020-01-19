#ifndef IMAGE_H
#define IMAGE_H

#include "structs.h"


extern PICImage picCreateImage(int w, int h, char * data);
extern void picDrawImage(PICImage img, float w, float h);
extern void picDeleteImage(PICImage img);

#endif