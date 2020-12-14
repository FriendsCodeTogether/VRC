/*
 * Car.h
 *
 */


#ifndef CAR_H_
#define CAR_H_

#include <avr/io.h>         /* Include AVR std. library file */
#include <debugBoard.h>     /* Include hardware library file */
#include <I2cConstants.h>   /* Include I2C library file */

void setupCar(void);                /* Setup car */
void setDirection(char direction);	/* set car steering dirtection */
void setThrottle(char throttle);	  /* set car throttle */
void setBuzzer(uint8_t value);	    /* set car buzzer */

#endif /* CAR_H_ */
