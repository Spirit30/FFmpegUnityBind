//
//  intercept.c
//  Unity-iPhone
//
//  Created by MaxBotvinev on 02.10.17.
//

#include <stdio.h>
#include <string.h>
#include "intercept.h"

#include "ffmpeg_wrapper.h"

int printf_redirect(const char *format, ...) {
    
    va_list arg;
    int done;
    
    va_start (arg, format);
    //vfprintf(stdout, "\nREDIRECT to Unity: \n", NULL);
    done = vfprintf(stdout, format, arg);
    
        int LOG_BUFFER_LENGTH = 1024;
        char buf[LOG_BUFFER_LENGTH];
        vsnprintf(buf, LOG_BUFFER_LENGTH, format, arg);
        on_progress(buf);
    va_end (arg);
    
    return done;
}
