/*
 * debugBoard.c
 *
 * Created: 11/11/2020 17:37:02
 *  Author: VRCar-Team
 */ 
#include <avr/io.h>
#include "debugBoard.h"

int previous, current;

void debugBoardSetup(void)
{
	DDRD = 0b11110000;
	PORTD = 0b00001111;
}

void debugBoardUpdate(void)
{
	previous = current;
	current = PIND;
}

int debugBoardButtonPressed(int button)
{
	if((PIND & (1<<button))==0) return 1;
	else return 0;
}

int debugBoardButtonIsBeingPressed(int button)
{
	if(((current & (1<<button))==0)  && ((previous & (1<<button))!=0)) return 1;
	else return 0;
}

void debugBoardShowBinaryNumber(int number)
{
	PORTD = (number<<4) | 0x0f;
}

void debugBoardSetLed(int led)
{
	PORTD = PORTD | (1<<led);
}

void debugBoardClearLed(int led)
{
	PORTD = PORTD & ~(1<<led);
}

void debugBoardToggleLed(int led)
{
	PORTD = PORTD ^ (1<<led);
}

void debugBoardClearAllLeds(void)
{
	debugBoardClearLed(LedWhite);
	debugBoardClearLed(LedGreen);
	debugBoardClearLed(LedRed);
	debugBoardClearLed(LedBlue);
}