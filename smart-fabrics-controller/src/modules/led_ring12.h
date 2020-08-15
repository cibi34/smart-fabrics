#include <Arduino.h>

// A basic everyday NeoPixel strip test program.

#include <Adafruit_NeoPixel.h>


// How many NeoPixels are attached to the Arduino?
int LED_COUNT = 12;

// Declare our NeoPixel strip object:
Adafruit_NeoPixel strip(LED_COUNT, 13, NEO_GRB + NEO_KHZ800);


// Rainbow cycle along whole strip. Pass delay time (in ms) between frames.
void rainbow(int wait) {

  for(long firstPixelHue = 0; firstPixelHue < 5*65536; firstPixelHue += 256) {
    for(int i=0; i<strip.numPixels(); i++) { 

      int pixelHue = firstPixelHue + (i * 65536L / strip.numPixels());

      strip.setPixelColor(i, strip.gamma32(strip.ColorHSV(pixelHue)));
    }
    strip.show(); // Update strip with new contents
    delay(wait);  // Pause for a moment
  }
}


// setup() function -- runs once at startup --------------------------------

void led_setup() {

  strip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  strip.show();            // Turn OFF all pixels ASAP
  strip.setBrightness(200); // Set BRIGHTNESS to about 1/5 (max = 255)
}


// loop() function -- runs repeatedly as long as board is on ---------------

void led_loop() {
  //rainbow(1);             // Flowing rainbow cycle along the whole strip
}

void led_rainbow(int w){
    rainbow(w);
    strip.clear();
    strip.show();
}

