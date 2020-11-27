#include <avr/io.h>
#include <lights.h>

void carSetup(void)
{
  lightsSetup();
}

void startup(void)
{
  lightsTest();
}

int main(void)
{
  carSetup();
  startup();
  while (1)
  {
  }
}
