/*
* hBridge.c
*
* Created: 2/11/2020 17:02:03
*  Author: VRCar-Team
*/
#include "hBridge.h"
#include <avr/io.h>
#include "pwm.h"
#include "debugBoard.h"

void driveForwards(void)
{
	PORTB = PORTB  & ~(1 << backwards);
	PORTB = PORTB | (1 << forwards);
}

void driveBackwards(void)
{
	PORTB = PORTB  & ~(1 << forwards);
	PORTB = PORTB | (1 << backwards);
}

void driveNeutral(void)
{
	PORTB = PORTB  & ~(1 << forwards);
	PORTB = PORTB  & ~(1 << backwards);
}

void steerLeft(void)
{
	PORTB = PORTB  & ~(1 << right);
	PORTB = PORTB | (1 << left);
}

void steerRight(void)
{
	PORTB = PORTB  & ~(1 << left);
	PORTB = PORTB | (1 << right);
}

void steerStraight(void)
{
	PORTB = PORTB  & ~(1 << left);
	PORTB = PORTB  & ~(1 << right);
}

void hBridgeSetup(void)
{
	DDRB = 0xff;
	PORTB =0b00010000;
	DDRD = DDRD | (1<<7);
	PORTD = PORTD | (1<<7);
	pwmSetup();
}

void driveCar(int direction, int throttle, int driveSpeed, int turnSpeed)
{
	pwmTurningDirection(turnSpeed);
	pwmDrivingSpeed(driveSpeed);
	
	if (direction == -1)steerLeft();
	else if (direction == 0)steerStraight();
	else if (direction == 1)steerRight();
	
	if (throttle == -1)driveBackwards();
	else if (throttle == 0)driveNeutral();
	else if (throttle == 1)driveForwards();
}