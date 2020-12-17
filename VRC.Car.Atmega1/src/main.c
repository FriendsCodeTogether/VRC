#include <avr/io.h>          /* Include AVR std. library file */
#include <util/delay.h>      /* Include inbuilt defined Delay header file */
#include <stdio.h>           /* Include standard I/O header file */
#include <string.h>          /* Include string header file */
#include <I2cSlave.h>        /* Include I2C slave header file */
#include <I2cConstants.h>    /* Include I2C constants header file */
#include <ByteConversions.h> /* Include Byte conversions header file */
#include <Car.h>             /* Include Car header file */

#define Slave_Address 0x20

uint8_t transmitQueue[8];
int speed = 90;

int main(void)
{
  I2cSlaveInit(Slave_Address);
  HBridgeSetup();

  while (1)
  {
    switch (I2cSlaveListen()) /* Check for any SLA+W or SLA+R */
    {
    case MASTER_WRITES_TO_SLAVE:
    {
      char received = 0;
      do
      {
        received = I2cSlaveReceive();

        if (received == MOTOR)
        {
          received = I2cSlaveReceive();
          char direction = received;

          received = I2cSlaveReceive();
          char throttle = received;
          driveCar(direction, throttle, speed);
        }

        if (received == COLOR_SENSOR)
        {
          int colorValue = 2;
          intToByteArray(colorValue, transmitQueue);
        }

        if (received == LIGHT_SENSOR)
        {
          uint8_t lightSensorValue = 1;
          transmitQueue[0] = lightSensorValue;
        }

        if (received == ULTRASONIC_SENSOR)
        {
          int ultrasonicValue = 5;
          intToByteArray(ultrasonicValue, transmitQueue);
        }
        if (received == BUZZER)
        {
          received = I2cSlaveReceive();
          setBuzzer(received);
        }
      } while (received != STOP_OR_REPEATED_START_RECEIVED); /* Receive until STOP/REPEATED START received */
      break;
    }
    case MASTER_READS_FROM_SLAVE:
    {
      unsigned char *transmitBuffer = transmitQueue;
      int8_t Ack_status;
      do
      {
        Ack_status = I2cSlaveTransmit(*transmitBuffer); /* Send data byte */
        transmitBuffer++;
      } while (Ack_status == 0); /* Send until Acknowledgment is received */
      break;
    }
    default:
      break;
    }
  }
}
