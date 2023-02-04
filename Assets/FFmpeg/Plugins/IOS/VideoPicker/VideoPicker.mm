//
//  VideoPicker.m
//  VideoPicker
//
//  Created by tanyamax on 19.09.17.
//  Copyright © 2017 MaxBotvinev. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MobileCoreServices/MobileCoreServices.h>
#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>

@implementation VideoPicker : NSObject

void * callback_ptr;
static const char * cVideoPath;

+ (VideoPicker*)instance
{
    static VideoPicker *instance = nil;
    if( !instance )
        instance = [[VideoPicker alloc] init];
    return instance;
}

- (void)init:(void(*)(const char*))_callback; {
    
    callback_ptr = (void*)_callback;
    [self pickVideo];
}

- (void)pickVideo {
    
    UIImagePickerController * picker = [[UIImagePickerController alloc] init];
    
    picker.delegate = self;
    picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    picker.mediaTypes = [[NSArray alloc] initWithObjects:(NSString *)kUTTypeMovie, nil];
    
    UIWindow *window=[UIApplication sharedApplication].keyWindow;
    UIViewController *vc = [window rootViewController];
    
    [vc presentModalViewController:picker animated:YES];
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    NSString * videoPath = [[info objectForKey:UIImagePickerControllerMediaURL] path];
    cVideoPath = strdup([videoPath UTF8String]);
    printf("Video path c string: %s%s", cVideoPath, "\n");
    
    [picker dismissViewControllerAnimated:YES completion:Nil];
    ((void(*)(const char*))callback_ptr)(cVideoPath);
}

- (void) play:(const char*)path {
    
    NSString * nsPath = [NSString stringWithUTF8String:path];
    NSURL * playURL = [NSURL URLWithString:nsPath];
    NSLog(@"Play URL: %@", playURL);
    
    AVPlayer *player = [AVPlayer playerWithURL:playURL];
    AVPlayerViewController *playerViewController = [AVPlayerViewController new];
    playerViewController.player = player;
    
    UIWindow *window=[UIApplication sharedApplication].keyWindow;
    UIViewController *vc = [window rootViewController];
    
    [vc  presentViewController:playerViewController animated:YES completion:nil];
}

@end

