/*
 * I2cSlave.h
 *
 */


#ifndef I2C_SLAVE_H_
#define I2C_SLAVE_H_

#include <avr/io.h> /* Include AVR std. library file */

#define SCL 0
#define SDA 1

void I2cSlaveInit(uint8_t slave_address);	/* I2C slave initialize function with Slave address */
int8_t I2cSlaveListen();					        /* I2C slave listen function */
int8_t I2cSlaveTransmit(char data);			  /* I2C slave transmit function */
char I2cSlaveReceive();						        /* I2C slave receive function */

#endif /* I2C_SLAVE_H_ */
