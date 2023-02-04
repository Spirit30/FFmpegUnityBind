//
//  intercept.h
//  Unity-iPhone
//
//  Created by MaxBotvinev on 02.10.17.
//

#ifndef intercept_h
#define intercept_h

#define printf(fmt, ...) printf_redirect(fmt, ##__VA_ARGS__)

int printf_redirect(const char *format, ...);

#endif /* intercept_h */
