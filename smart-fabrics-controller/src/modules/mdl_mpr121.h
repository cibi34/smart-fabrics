#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_MPR121.h>

#ifndef _BV
#define _BV(bit) (1 << (bit))
#endif

Adafruit_MPR121 cap = Adafruit_MPR121();

uint16_t lasttouched = 0;
uint16_t currtouched = 0;

void mpr_setup()
{

  // Default address is 0x5A, if tied to 3.3V its 0x5B
  // If tied to SDA its 0x5C and if SCL then 0x5D
  if (!cap.begin(0x5A))
  {
    Serial.println("MPR121 not found, check wiring?");
    while (1)
      ;
  }
  Serial.println("MPR121 found!");
}

void mpr_loop()
{
  // Get the currently touched pads
  currtouched = cap.touched();

  // for (uint8_t i=0; i<12; i++) {
  //   // it if *is* touched and *wasnt* touched before
  //   if ((currtouched & _BV(i)) && !(lasttouched & _BV(i)) ) {
  //     Serial.print(i); Serial.println(" touched");
  //   }
  //   // if it *was* touched and now *isnt*
  //   if (!(currtouched & _BV(i)) && (lasttouched & _BV(i)) ) {
  //     Serial.print(i); Serial.println(" released");
  //   }
  // }

  // reset state
  lasttouched = currtouched;

  // comment out this line for detailed data from the sensor!
  //return;

  // debugging info, what
  //Serial.print("Touched: 0x"); Serial.println(cap.touched(), HEX);

  for (uint8_t i = 0; i < 12; i++)
  {
    Serial.print(i);
    Serial.print(":");
    Serial.print(cap.baselineData(i));
    Serial.print(":");
    Serial.println(cap.filteredData(i));
  }

  delay(100);
}
