//
//  FileLogger.m
//  Unity-iPhone
//
//  Created by MaxBotvinev on 03.10.17.
//

#import <Foundation/Foundation.h>

@interface FileLogger : NSObject

+ (void) WriteToFile:(const char *)msg;
+ (const char * ) ReadFromFile;
+ (void) ClearFile;
@end

//INTERFACE
//------------------------------

void write_to_file(const char * msg) {
    
    [FileLogger WriteToFile:msg];
}

const char * read_from_file() {
    
    return [FileLogger ReadFromFile];
}

void clear_file() {
    
    [FileLogger ClearFile];
}

//------------------------------

@implementation FileLogger

+ (NSString * )GetLogPath {
    
    NSArray * searchPaths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString * documentPath = [searchPaths objectAtIndex:0];
    return [documentPath stringByAppendingString:@"/ffmpeg_log_to_unity_bind.txt"];
}

+ (void) WriteToFile:(const char *)msg {
    
    NSString * path = [self GetLogPath];
    NSFileHandle *fileHandle = [NSFileHandle fileHandleForWritingAtPath:path];
    [fileHandle seekToEndOfFile];
    NSString * log = [NSString stringWithUTF8String:msg];
    [fileHandle writeData:[log dataUsingEncoding:NSUTF8StringEncoding]];
    [fileHandle closeFile];
}

+ (const char * ) ReadFromFile {
    
    NSString * path = [self GetLogPath];
    NSString* content = [NSString stringWithContentsOfFile:path encoding:NSUTF8StringEncoding error:NULL];
    return [content UTF8String];
}

+ (void) ClearFile {
    
    [[NSFileManager defaultManager] createFileAtPath:[self GetLogPath] contents:[NSData data] attributes:nil];
}

@end
