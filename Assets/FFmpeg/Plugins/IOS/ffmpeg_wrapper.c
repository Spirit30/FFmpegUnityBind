//  ffmpeg_wrapper.c
//  FFmpegTest_4
//
//  Created by tanyamax on 16.09.17.
//  Copyright © 2017 MaxBotvinev. All rights reserved.
//

#include "ffmpeg_wrapper.h"
#include "c_string.h"
#include "FileLogger/FileLogger.h"

//Interface
#ifdef __cplusplus
extern "C" {
#endif
    void *execute(char ** argv, int argc, void (*callback)(const char*));
#ifdef __cplusplus
}
#endif

struct pthread_data {
    
    int argc;
    char ** argv;
};

struct pthread_data * data;

const char * log_buffer;

//CALLBACKS
//------------------------------

void main_callback(int error, char * event, const char * msg) {
    
    //Clear
    if((char *)log_buffer)
        free((char *)log_buffer);
    
    //Prepare
    const char * ERROR_KEY =    "FFmpeg EXCEPTION: ";
    const char * COMMAND_KEY =  "FFmpeg COMMAND: ";
    
    //Unsafe
    const char * prefix = append(error ? ERROR_KEY : COMMAND_KEY, event);
    
    //File Ops
    write_to_file(msg);
    const char * suffix = read_from_file();
    
    //Unsafe
    log_buffer = append(prefix, suffix);
    free((char *)prefix);
    
    //Output
    callback_ptr(log_buffer);
}

void on_start() {
    
    main_callback(0, "onStart", "\nStarted\n");
}

void on_progress(const char * msg) {
    
    main_callback(0, "onProgress: ", msg);
}

void on_failure() {
    
    main_callback(0, "onFailure: ", "Failure. Search details above.\n");
}

void on_success() {
    
    main_callback(0, "onSuccess: ", "Success!\n");
}

void on_finish() {
    
    free(data->argv);
    free(data);
    main_callback(0, "onFinish", "\nFinished\n");
}

void error_callback(const char * msg) {
    
    main_callback(1, "\nError:\n", msg);
}

//------------------------------

//LUNCH
//------------------------------

void * pthread_execute(void * _data) {
    
    data = (struct pthread_data *)_data;
    
    ffmpeg_main(
                data->argc,
                data->argv);
    
    return NULL;
}

//------------------------------

//INTERFACE
//------------------------------

void *execute(char ** argv, int argc, void (*callback)(const char*)) {
    
    //Clear Output Data
    clear_file();
    
    //Copy Input Data
    int arguments_count = argc + 1;
    char ** arguments = calloc(arguments_count, sizeof(char*));
    arguments[0] = "ffmpeg";
    
    int a1, a2;
    printf("\nC side Arguments:\n");
    for(a1 = 0, a2 = 1; a1 < argc; a1++, a2++) {
        
        arguments[a2] = strdup(argv[a1]);
        printf("%d Arg: %s\n", a2, arguments[a2]);
    }
    
    //Open thread
    pthread_t thread;
    
    struct pthread_data * data = calloc(argc, sizeof(char*));
    
    data->argv = arguments;
    data->argc = arguments_count;
    
    callback_ptr = callback;
    
    //Create a thread and exectute
    pthread_create(&thread, NULL, pthread_execute, (void *)data);
    
    return NULL;
}

//------------------------------

