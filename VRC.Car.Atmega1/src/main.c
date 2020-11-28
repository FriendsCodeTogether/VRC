#include <avr/io.h>
#include <avr/interrupt.h>
#include "util/delay.h"

#include <lights.h>
#include <debugBoard.h>
#include <l298hn.h>
#include <uart.h>

/*VARIABLES*/

char buffrx[100];
int rxpnt = 0;
int messageReceived = 0;
int driveSpeed = 100; //in %
int turnSpeed = 100;  //in %
int previousThrottle = 1;

/*FUNCTIONS*/

void debugMode(void)
{
  debugBoardSetup();
}

void carSetup(void)
{
  lightsSetup();
  hBridgeSetup();
  uartSetup();
  sei();
}

void startup(void)
{
  lightsTest();

  lightsPositionLights();
}

ISR(USART_RXC_vect)
{
  buffrx[rxpnt] = UDR;
  uartSendByte(buffrx[rxpnt]); //DEBUG CODE
  if (buffrx[rxpnt] == 36)     // stop when you see a $
  {
    messageReceived = 1;
    buffrx[rxpnt] = 0;
    rxpnt = 0;
  }
  else
    rxpnt++;
}

int main(void)
{
  //debugMode();
  carSetup();
  startup();
  while (1)
  {
    if (messageReceived == 1)
    {
      lightsClearIndicators();
      if (stringCompare("f", buffrx) || stringCompare("F", buffrx)) //forwards
      {
        driveCar(0, 1, driveSpeed, turnSpeed);
        previousThrottle = 1;
      }
      else if (stringCompare("b", buffrx) || stringCompare("B", buffrx)) //backwards
      {
        driveCar(0, -1, driveSpeed, turnSpeed);
        previousThrottle = -1;
      }
      else if (stringCompare("l", buffrx) || stringCompare("L", buffrx)) //left
      {
        lightsLeftIndicator();
        driveCar(-1, previousThrottle, driveSpeed, turnSpeed);
      }
      else if (stringCompare("r", buffrx) || stringCompare("R", buffrx)) //right
      {
        lightsRightIndicator();
        driveCar(1, previousThrottle, driveSpeed, turnSpeed);
      }
      else
      {
        driveCar(0, 0, 0, 0); //Stop car
        previousThrottle = 0;
      }
      messageReceived = 0;
    }
  }
}
