#include <avr/io.h>
#include <avr/interrupt.h>
#include "util/delay.h"
#include <hcsr05.h>
#include <buzzer.h>
#include <SSD1306.h>

void carSetup(void)
{
  ultrasonicSetup();
  buzzerSetup();
  OLED_Init();
  sei();
}

int main(void)
{
  carSetup();
  OLED_Clear();
  OLED_SetCursor(0, 0);
  OLED_DisplayNumber(C_DECIMAL_U8, 15, C_MaxDigitsToDisplay_U8);
  while (1)
  {
    ultraTriger(1);
  }
}
