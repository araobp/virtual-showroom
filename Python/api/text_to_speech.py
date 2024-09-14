from openai import OpenAI

client = OpenAI()

class TTS:

    def __init__(self, voice:str="alloy") -> None:
        self.voice = voice

    # Outputs voice data in mp3 format
    def speak(self, input:str, filepath:str=None) -> bytes:
        response = client.audio.speech.create(
            model="tts-1",
            voice=self.voice,
            input=input,
        )
        if filepath is not None:
            response.write_to_file(filepath)

        return response.read()

if __name__ == '__main__':
    alloy = TTS(voice='alloy')
    nova = TTS(voice='nova')

    #alloy.speak(input='I believe the children are our future.', filepath='tmp/test_alloy_en.mp3')
    nova.speak(input='I believe the children are our future.', filepath='tmp/test_nova_en.mp3')
    #alloy.speak(input='私は子供達が私たちの未来であると信じます。', filepath='tmp/test_alloy_ja.mp3')
    #nova.speak(input='私は子供達が私たちの未来であると信じます。', filepath='tmp/test_nova_ja.mp3')

    data = nova.speak(input='I believe the children are our future.', filepath='tmp/test_nova_en.mp3')
    with open('tmp/test_nova_en_raw.mp3', 'wb') as f:
        f.write(data)