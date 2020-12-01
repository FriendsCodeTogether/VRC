#include <hcsr05.h>
#include <buzzer.h>
#include <debugBoard.h>
#include <SSD1306.h>

int sensor_working = 0;
int rising_edge = 0;
int timer_counter = 0;
int distance;
int goingBack;

void ultrasonicSetup(void)
{
  TRIGER_DDR |= (1 << TRIGER);
  ECHO_DDR &= ~(1 << ECHO);
  ECHO_PULLUP |= (1 << ECHO);
  enableExInterrupt();
  timer0_init();
}

void enableExInterrupt(void)
{
  MCUCR |= (1 << ISC10); // Trigger INT1 on any logic change.
  GICR |= (1 << INT1);   // Enable INT1 interrupts.
}

void ultraTriger(int goingBackwards)
{
  goingBack = goingBackwards;

  if (!sensor_working)
  {
    TRIGER_PORT |= (1 << TRIGER);
    _delay_us(15);
    TRIGER_PORT &= ~(1 << TRIGER);
    sensor_working = 1;
  }
}

void detection(void)
{
  if (goingBack)
  {
    buzzerWinningSound();
    //Do something on car (lights or buzzer sound)
  }
  else
  {
    //SendMessage(im being followed)
  }
}

ISR(INT1_vect)
{
  if (sensor_working == 1)
  {
    if (rising_edge == 0)
    {
      TCNT0 = 0x00;
      rising_edge = 1;
      timer_counter = 0;
    }
    else
    {
      distance = (timer_counter * 256 + TCNT0) / 466;
      if (distance < 20 && distance > 0)
      {
        //OLED_DisplayString('distance: ');
        OLED_SetCursor(1, 0);
        OLED_DisplayNumber(C_DECIMAL_U8, distance, C_MaxDigitsToDisplay_U8);
        detection();
        timer_counter = 0;
        rising_edge = 0;
      }
    }
  }
}

ISR(TIMER0_OVF_vect)
{
  timer_counter++;
  if (timer_counter > 730)
  {
    TCNT0 = 0x00;
    sensor_working = 0;
    rising_edge = 0;
    timer_counter = 0;
  }
}
