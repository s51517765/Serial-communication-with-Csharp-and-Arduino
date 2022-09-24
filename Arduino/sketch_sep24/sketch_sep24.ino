int SerialRead = 0;
int SwPin = A0;
void setup() {
  Serial.begin(9600);
  pinMode(SwPin, INPUT_PULLUP);
}

int digit = 0;
int count = 0;
int inByte[3];

void loop() {
  SerialRead = Serial.available();  // シリアルポートにデータがあるかを確認
  if (SerialRead > 0)
  {
    // Serial.print("L");
    inByte[digit++] = Serial.read(); // データを読み込む

    if (inByte[digit - 1] == 47)
    {
      //3桁のデータを受信したら、文字列を数値に
      count += (inByte[0] - 48) * 100;
      count += (inByte[1] - 48) * 10;
      count += inByte[2] - 48;

      for (int i = 0; i < count; i++) {
        digitalWrite(LED_BUILTIN, HIGH);
        delay(300);
        digitalWrite(LED_BUILTIN, LOW);
        delay(300);
      }
      Serial.print(count);
      Serial.print('\0');

      delay(1000);  // 通信用の待機時間
      count = 0;
      digit = 0;
    }
  }
  if (digitalRead(SwPin) == LOW) {
    delay(400);
    while (digitalRead(SwPin) == LOW) {}
    Serial.print("Switch pushed.");
    Serial.print('\0');
  }

}
