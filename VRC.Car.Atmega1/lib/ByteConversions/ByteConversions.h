/*
 * ByteConversions.h
 *
 */ 


#ifndef BYTE_CONVERSIONS_H_
#define BYTE_CONVERSIONS_H_

#include <avr/io.h>     /* Include AVR std. library file */
#include <string.h>     /* Include string header file */

void intToByteArray(int val,uint8_t* bytes_array);          /* Copy int value into byte[] */
void longToByteArray(long val,uint8_t* bytes_array);		/* Copy long value into byte[] */
void floatToByteArray(float val, uint8_t* bytes_array);	    /* Copy double value into byte[] */
void doubleToByteArray(double val, uint8_t* bytes_array);	/* Copy double value into byte[] */			

#endif /* BYTE_CONVERSIONS_H_ */