//
//  FileLogger.h
//  Unity-iPhone
//
//  Created by Max Botvinev on 03.10.17.
//

#ifndef FileLogger_h
#define FileLogger_h

void write_to_file(const char * msg);
const char * read_from_file();
void clear_file();

#endif /* FileLogger_h */
