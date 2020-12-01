#include <avr/io.h>
#include "util/delay.h"
#include <buzzer.h>

void buzzerSetup(void)
{
  DDRD |= (1 << buzzer);
  PORTD |= ~(1 << buzzer);
}

void buzzerRearDetection(void)
{
  buzzerOn();
  _delay_ms(250);
  buzzerOff();
  _delay_ms(250);
}

void buzzerWinningSound(void)
{
  buzzerOn();
  _delay_ms(250);
  buzzerOff();
  _delay_ms(500);
  buzzerOn();
  _delay_ms(300);
  buzzerOff();
  _delay_ms(250);
}

void buzzerOn(void)
{
  PORTD = PORTD | (1 << buzzer);
}

void buzzerOff(void)
{
  PORTD = PORTD & ~(1 << buzzer);
}
