//
//  VideoPicker.h
//  VideoPicker
//
//  Created by tanyamax on 19.09.17.
//  Copyright © 2017 MaxBotvinev. All rights reserved.
//

#ifndef VideoPicker_h
#define VideoPicker_h
#import <Foundation/Foundation.h>

@interface VideoPicker : NSObject { }
+ (VideoPicker*)instance;
- (void)init:(void(*)(const char*))callback;
- (void) play:(const char*)path;
@end

#endif /* VideoPicker_h */

