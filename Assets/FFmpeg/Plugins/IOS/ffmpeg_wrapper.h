//
//  ffmpeg_wrapper.h
//  FFmpegTest_4
//
//  Created by MaxBotvinev on 16.09.17.
//  Copyright © 2017 MaxBotvinev. All rights reserved.
//

#ifndef ffmpeg_wrapper_h
#define ffmpeg_wrapper_h

#import "ffmpeg.h"

const int LOG_BUFFER_LENGTH = 1024;

//INTERFACE
void *execute(char ** argv, int argc, void (*callback)(const char*));

//CALLBACKS
static void (*callback_ptr)(const char*msg);

void main_callback(int error, char * event, const char * msg);
void on_start();
void on_progress(const char * msg);
void on_failure();
void on_success();
void on_finish();
void error_callback(const char * msg);

#endif /* ffmpeg_wrapper_h */
