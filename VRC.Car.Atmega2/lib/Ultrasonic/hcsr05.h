#ifndef _ULTRA_H_
#define _ULTRA_H_

#include <avr/io.h>
#include <avr/interrupt.h>
#include <string.h>
#include <stdlib.h>
#include <util/delay.h>
#include "timer.h"

#define TRIGER_DDR DDRD
#define ECHO_DDR DDRD
#define TRIGER_PORT PORTD
#define ECHO_PULLUP PORTD
#define TRIGER 0
#define ECHO 3

/*************************************************
 *  API functions
 *************************************************/

void ultrasonicSetup(void);
void enableExInterrupt(void);
void ultrasonicTriger(int goingBackwards);

#endif
