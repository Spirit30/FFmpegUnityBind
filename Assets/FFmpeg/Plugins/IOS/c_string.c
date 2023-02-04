//
//  c_string.c
//  Unity-iPhone
//
//  Created by Max Botvinev on 03.10.17.
//

#include <string.h>
#include <stdlib.h>
#include "c_string.h"

//Unsafe result
const char * append(const char * a, const char * b) {
    
    size_t size1 = strlen(a);
    size_t size2 = strlen(b);
    size_t size = sizeof(char) * (size1 + size2 + 1);
    
    char * append_buffer = malloc(size);
    memcpy(append_buffer, a, size1);
    memcpy(append_buffer + size1, b, size2);
    append_buffer[size1 + size2] = '\0';
    
    return append_buffer;
}
