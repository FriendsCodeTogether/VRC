/*
 * Car.c
 *
 */

#include "Car.h"

void setupCar(void)
{
    debugBoardSetup();
}

void setDirection(char direction)
{
	switch (direction)
	{
		case DIRECTION_LEFT:
			debugBoardClearLed(LedGreen);
			debugBoardSetLed(LedRed);
			break;
		case DIRECTION_RIGHT:
			debugBoardClearLed(LedRed);
			debugBoardSetLed(LedGreen);
			break;
		case DIRECTION_NEUTRAL:
			debugBoardClearLed(LedRed);
			debugBoardClearLed(LedGreen);
			break;
		default:
			break;
	}
}

void setThrottle(char throttle)
{
	switch (throttle)
	{
		case THROTTLE_FORWARD:
			debugBoardClearLed(LedWhite);
			debugBoardSetLed(LedBlue);
			break;
		case THROTTLE_BACKWARD:
			debugBoardClearLed(LedBlue);
			// Execute order 66
			debugBoardSetLed(LedWhite);
			break;
		case THROTTLE_NEUTRAL:
			debugBoardClearLed(LedBlue);
			debugBoardClearLed(LedWhite);
			break;
		default:
			break;
	}
}

void setBuzzer(uint8_t value)
{
	if (value == 0)
	{
		// turn off buzzer
		debugBoardClearLed(LedRed);
	} else {
		// turn on buzzer
		debugBoardSetLed(LedRed);
	}
}