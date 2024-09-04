from flask import Flask, request, jsonify
import azure.cognitiveservices.speech as speechsdk

app = Flask(__name__)

# Azure Cognitive Services Speech SDK 설정
speech_key = "..."
service_region = "..."
speech_config = speechsdk.SpeechConfig(subscription=speech_key, region=service_region)
audio_config = speechsdk.audio.AudioOutputConfig(use_default_speaker=True)
speech_config.speech_synthesis_voice_name = "ko-KR-InJoonNeural"

@app.route('/speak', methods=['POST'])
def speak():
    data = request.json
    text = data.get('text', '')

    if text:
        speech_synthesizer = speechsdk.SpeechSynthesizer(speech_config=speech_config, audio_config=audio_config)
        result = speech_synthesizer.speak_text_async(text).get()
        
        if result.reason == speechsdk.ResultReason.SynthesizingAudioCompleted:
            return jsonify({'status': 'success', 'message': f"Speech synthesized for text [{text}]"}), 200
        elif result.reason == speechsdk.ResultReason.Canceled:
            cancellation_details = result.cancellation_details
            error_message = f"Speech synthesis canceled: {cancellation_details.reason}"
            if cancellation_details.reason == speechsdk.CancellationReason.Error:
                error_message += f", Error details: {cancellation_details.error_details}"
            return jsonify({'status': 'error', 'message': error_message}), 500
    else:
        return jsonify({'status': 'error', 'message': 'No text provided'}), 400

if __name__ == '__main__':
    app.run(port=6001)
