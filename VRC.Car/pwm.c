/*
* pwm.c
*
* Created: 2/11/2020 16:56:27
*  Author: VRCar-Team
*/
#include "pwm.h"
#include <avr/io.h>

void pwmSetup(void)
{
	TCCR0 = (1<<WGM00)|(1<<WGM01)|(1<<COM01)|(1<<CS01);
	TCCR2 = (1<<WGM20)|(1<<WGM21)|(1<<COM21)|(1<<CS21);
}
void pwmDrivingSpeed(int drivingSpeed)
{
	//OCR0 = 250;
	OCR0 = (drivingSpeed * 255)/100;
}
void pwmTurningDirection(int turningDirection)
{
	//OCR2 = 250;
	OCR2 = (turningDirection * 255)/100;
}