/*
 * Car.c
 *
 */

#include "Car.h"

void driveCar(char direction, char throttle, int speed)
{
  pwmSpeed(speed);

  if (throttle == 'N')
  {
    stopCar();
  }
  else if (direction == 'N' && throttle == 'F')
  {
    driveStraightForwards();
  }
  else if (direction == 'N' && throttle == 'B')
  {
    driveStraightBackwards();
  }
  else if (direction == 'L' && throttle == 'F')
  {
    driveLeftForwards();
  }
  else if (direction == 'L' && throttle == 'B')
  {
    driveLeftBackwards();
  }
  else if (direction == 'R' && throttle == 'F')
  {
    driveRightForwards();
  }
  else if (direction == 'R' && throttle == 'B')
  {
    driveRightBackwards();
  }
}

void setBuzzer(uint8_t value)
{
  if (value == 0)
  {
    // turn off buzzer
    debugBoardClearLed(LedRed);
  }
  else
  {
    // turn on buzzer
    debugBoardSetLed(LedRed);
  }
}
