#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <bcm_host.h>
#include <EGL/egl.h>

#include "eglstate.h"
#include "ovgwrapper.h"
#include "font.h"

static STATE_T _state, *state = &_state;	// global graphics state

static int init_x = 0;		// Initial window position and size
static int init_y = 0;
static unsigned int init_w = 0;
static unsigned int init_h = 0;

extern PICSize picInit() {

  // init hardware + dispmanx + egl
	bcm_host_init();
	memset(state, 0, sizeof(*state));
	state->window_x = init_x;
	state->window_y = init_y;
	state->window_width = init_w;
	state->window_height = init_h;
	oglinit(state);

  // init ovg wrapper
  ovg_init(state->display, state->surface);

  // init font module
	font_lib_init();

  PICSize size;
  size.x = (float)state->window_width;
  size.y = (float)state->window_height;

  return size;
}

extern void picDispose(){
  eglSwapBuffers(state->display, state->surface);
  eglMakeCurrent(state->display, EGL_NO_SURFACE, EGL_NO_SURFACE, EGL_NO_CONTEXT);
  eglDestroySurface(state->display, state->surface);
  eglDestroyContext(state->display, state->context);
  eglTerminate(state->display);

  //dispose font module
	font_lib_dispose();
}

