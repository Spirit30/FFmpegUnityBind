//
//  VideoPickerBridge.c
//  VideoPicker
//
//  Created by tanyamax on 19.09.17.
//  Copyright © 2017 MaxBotvinev. All rights reserved.
//

#import "VideoPicker.h"

extern "C"
{
    //Interface
    void get_video_path(void (* callback)(const char*));
    void play(const char * path);
}

void get_video_path(void (* callback)(const char*)) {
    
    [[VideoPicker instance] init:callback];
}

void play(const char * path) {
    
    [[VideoPicker instance] play:path];
}

