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
  DDRD = DDRD | ~(1 << LDR);
  PORTA = 0x00;
  PORTD = DDRD | (1 << LDR);
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
  lightsClearLed(LeftIndicator);
  lightsClearLed(RightIndicator);
  lightsClearLed(BrakeLights);
  lightsClearLed(RearLights);
}

void ReadLDR(void)
{
  if ((PIND & (1 << LDR)) == 0)
    lightsClearLed(BigLights);
  else
    lightsSetLed(BigLights);
}