/*
 * Lightsensor.c
 *
 * Created: 26/11/2018 12:05:2020
 *  Author: u0036181
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>
#include "Lightsensor.h"

ISR(INT0_vect)
{
	GetLightvalue();
}

void LightSensorSetup()
{
	DDRD = ~(1<<PD2);
	PORTD = (1<<PD2);
	
	GICR = (1<<INT0);
	MCUCR = (1<<ISC00);
}
int GetLightvalue()
{
	if (PD2 == 0)
	{
		return 1;
	} 
	else
	{
		return 0;
	}
}