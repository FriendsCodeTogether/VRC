/*
 * lights.c
 *
 * Created: 12/11/2020 0:33:28
 *  Author: VRC-Team
 */

#include <avr/io.h>
#include "lights.h"
#include <util/delay.h>

void lightsSetup(void)
{
  DDRA = 0xff;
  PORTA = 0x00;
}

void lightsSetLed(int led)
{
  PORTA = PORTA | (1 << led);
}

void lightsClearLed(int led)
{
  PORTA = PORTA & ~(1 << led);
}

void lightsToggleLed(int led)
{
  PORTA = PORTA ^ (1 << led);
}

void lightsClearAllLeds(void)
{
  PORTA = 0x00;
  lightsSetLed(PositionLights);
}

void testLights(void)
{
  //TODO
}