/*
* VRCLegoCar.c
*
* Created: 2/11/2020 13:30:25
* Author : VRCar-Team
*/

#define F_CPU 16000000
#include <avr/io.h>
#include <avr/interrupt.h>
#include "util/delay.h"
#include "debugBoard.h"
#include "uart.h"
#include "lights.h"
#include "hBridge.h"

char buffrx[100];
int rxpnt = 0;
int messageReceived;
int driveSpeed = 100;//in %
int turnSpeed = 100;//in %
int previousThrottle = 1;

void carSetup()
{
	hBridgeSetup();  // PORTB + 1pin PORTD
	uartSetup();
	lightsSetup(); //PortA
	sei();
}

ISR(USART_RXC_vect)
{
	buffrx[rxpnt] = UDR;
	uartSendByte(buffrx[rxpnt]); //DEBUG CODE
	if(buffrx[rxpnt] == 36) // stop when you see a $
	{
		messageReceived = 1;
		buffrx[rxpnt] = 0;
		rxpnt = 0;
	}
	else rxpnt++;
}

int main(void)
{
	carSetup();			
	lightsPositionLights();
	
	while (1)
	{
		if(messageReceived == 1)
		{
			lightsClearIndicators();
			
			if (stringCompare("f",buffrx)||stringCompare("F",buffrx)) //forwards
			{
				driveCar(0,1,driveSpeed,turnSpeed);
				previousThrottle = 1;
			}
			else if (stringCompare("b",buffrx)||stringCompare("B",buffrx)) //backwards
			{
				driveCar(0,-1,driveSpeed,turnSpeed);
				previousThrottle = -1;
			}
			else if (stringCompare("l",buffrx)||stringCompare("L",buffrx)) //left
			{
				lightsLeftIndicator();
				driveCar(-1, previousThrottle, driveSpeed,turnSpeed);
			}
			else if (stringCompare("r",buffrx)||stringCompare("R",buffrx)) //right
			{
				lightsRightIndicator();
				driveCar(1, previousThrottle, driveSpeed,turnSpeed);
			}
			else
			{
				driveCar(0,0,0,0); //Stop car
				previousThrottle = 0;
			}
			
			messageReceived = 0;
		}
	}
}