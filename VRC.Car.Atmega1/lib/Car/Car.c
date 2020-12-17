/*
 * Car.c
 *
 */

#include "Car.h"

void CarSetup(void)
{
  HBridgeSetup();
  lightsSetup();
}

void driveCar(char direction, char throttle, int speed)
{
  pwmSpeed(speed);
  lightsClear();
  if (throttle == 'N')
  {
    stopCar();
    lightsSetLed(BrakeLights);
  }
  else if (direction == 'N' && throttle == 'F')
  {
    driveStraightForwards();
  }
  else if (direction == 'N' && throttle == 'B')
  {
    driveStraightBackwards();
    lightsSetLed(RearLights);
  }
  else if (direction == 'L' && throttle == 'F')
  {
    driveLeftForwards();
    lightsSetLed(LeftIndicator);
  }
  else if (direction == 'L' && throttle == 'B')
  {
    driveLeftBackwards();
    lightsSetLed(LeftIndicator);
    lightsSetLed(RearLights);
  }
  else if (direction == 'R' && throttle == 'F')
  {
    driveRightForwards();
    lightsSetLed(RightIndicator);
  }
  else if (direction == 'R' && throttle == 'B')
  {
    driveRightBackwards();
    lightsSetLed(RightIndicator);
    lightsSetLed(RearLights);
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
