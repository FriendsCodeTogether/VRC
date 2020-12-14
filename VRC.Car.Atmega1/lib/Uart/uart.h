/*
 * uart.h
 *
 * Created: 2/11/2020 14:02:08
 *  Author: VRCar-Team
 */


#ifndef UART_H_
#define UART_H_

void uartSetup(void);

void uartSendByte(char value);

void uartSendString(char *txt);

int stringCompare(char * pnt1,char * pnt2);

#endif /* UART_H_ */
