/*
 * Car.h
 *
 */

#ifndef CAR_H_
#define CAR_H_

#include <avr/io.h>       /* Include AVR std. library file */
#include <debugBoard.h>   /* Include hardware library file */
#include <I2cConstants.h> /* Include I2C library file */
#include <L298HN.h>       /* Include HBridge library file */
#include <pwm.h>          /* Include PWM library file */
#include <lights.h>       /* Include Lights Library file */

void setupCar(void);                                     /* Setup car */
void setBuzzer(uint8_t value);                           /* set car buzzer */
void driveCar(char direction, char throttle, int speed); /* set car steering */
#endif                                                   /* CAR_H_ */
