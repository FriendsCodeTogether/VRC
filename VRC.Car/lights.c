/*
 * lights.c
 *
 * Created: 12/11/2020 0:33:28
 *  Author: VRC-Team
 */ 

#include <avr/io.h>
#include "lights.h"

void lightsSetup(void)
{
	DDRA = 0xff;
	PORTA = 0x00;
}

void lightsSetLed(int led)
{
	PORTA = PORTA | (1<<led);
}

void lightsClearLed(int led)
{
	PORTA = PORTA & ~(1<<led);
}

void lightsToggleLed(int led)
{
	PORTA = PORTA ^ (1<<led);
}

void lightsClearAllLeds(void)
{
	PORTA = 0x00;
}

void lightsClearIndicators(void)
{
	lightsClearLed(LeftIndicator);
	lightsClearLed(RightIndicator);
}

void lightsClearFrontBar(void)
{
	lightsClearLed(FrontBarMiddleLeft);
	lightsClearLed(FrontBarMiddleRight);
	lightsClearLed(FrontBarRight);
	lightsClearLed(FrontBarLeft);
}

void lightsClearPositionLights(void)
{
	lightsClearLed(FrontLeft);
	lightsClearLed(FrontRight);
}

void lightsFrontBar(void)
{
	lightsSetLed(FrontBarMiddleLeft);
	lightsSetLed(FrontBarMiddleRight);
	lightsSetLed(FrontBarRight);
	lightsSetLed(FrontBarLeft);
}

void lightsPositionLights(void)
{
	lightsSetLed(FrontLeft);
	lightsSetLed(FrontRight);
}

void lightsLeftIndicator(void)
{
	lightsSetLed(LeftIndicator);
}

void lightsRightIndicator(void)
{
	lightsSetLed(RightIndicator);
}