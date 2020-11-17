/*
 * uart.c
 *
 * Created: 2/11/2020 14:01:54
 *  Author: VRCar-Team
 */ 
#include <avr/io.h>
#include "uart.h"

void uartSetup(void)
{
	UCSRB = (1<<TXEN)|(1<<RXEN)|(1<<RXCIE);
	UBRRL = 51;
}

void uartSendByte(char value)
{
	while((UCSRA & (1<<UDRE))==0);
	UDR = value;
}

void uartSendString(char *txt)
{
	while(*txt != 0)
	{
		uartSendByte(*txt);
		txt++;
	}
}

int stringCompare(char * pnt1,char * pnt2)
{
	while((*pnt1 !=0))
	{
		if(*pnt1 != *pnt2)return 0;
		pnt1++;
		pnt2++;
	}
	return 1;
}
