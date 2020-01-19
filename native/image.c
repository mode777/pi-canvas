#include <stdlib.h>
#include "VG/openvg.h"
#include "VG/vgu.h"	
#include "image.h"

static VGfloat matrix[9]; 

PICImage picCreateImage(int w, int h, char * data) {
	unsigned int dstride = w * 4;
	VGImageFormat rgbaFormat = VG_sRGBA_8888;
	VGImage img = vgCreateImage(rgbaFormat, w, h, VG_IMAGE_QUALITY_BETTER);
	vgImageSubData(img, (void *)data, dstride, rgbaFormat, 0, 0, w, h);
    return (PICImage)img;
}

void picDrawImage(PICImage img, float x, float y){
    vgGetMatrix(matrix);
    vgSeti(VG_MATRIX_MODE, VG_MATRIX_IMAGE_USER_TO_SURFACE);
    vgLoadMatrix(matrix);
    vgTranslate(x, y);
    
    vgDrawImage(img);

    vgSeti(VG_MATRIX_MODE, VG_MATRIX_PATH_USER_TO_SURFACE);
}

void picDeleteImage(PICImage img){
    vgDestroyImage(img);
}