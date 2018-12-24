# Cognitive Text-to-Speech API: Article Reader App

## Overview
This is a great time to be developer. One can create smart solutions & solve everyday problems by integrating cloud based APIs with your applicaion. Speech to Text API from Microsoft is a cognitive service which provides natural text to speech capability. The News Article Reader app demonstrates how one can create a news reading app using Text-to-Speech API in no time. Host it in Azure & you can listen to the news article of your choice, which you wanted to read in your busy day, on your drive home from office. The app can be seen in action by clicking the link below.

[News article reader app](https://newsarticlereader.azurewebsites.net/)

The app can be used on any browser and any device. Build it on .Net core & you can deploy it on any platform - linux, windows or mac OS.

## How it works?
The Text-to-Speech Cognitive service from Microsoft accepts HTTP request and returns response as an audio file.

The voice synthesis API requires a JSON web Token(JWT) which is passed as a bearer token with the request. The JWT token is issued by the token endpoint of the Text-to-Speech API. You can subscribe & obtain API keys to retrieve valid JWT access token using [Cognitive Services Subscription.](https://azure.microsoft.com/en-us/try/cognitive-services/)

The request is then sent as a HTTP POST call to the Text-to-Speech API's synthesize end point. The body of the request contains Speech Synthesis Markup Language (SSML) input that represents the text to be synthesized. SSML gives the ability to specify audio characteristics like pronunciation, volume, pitch etc. The response returned is an audio file.

The high level data flow between the ArticleReader app and the Text to Speech API is shown below.

![Alt text](/images/TextToSpeech-UML.JPG)

The ArticleReader app needs to extract article from a given url. You can use Text Analysis API (3rd party or Microsoft) to extract the news content from it. ArticleReader app is using Text Anlysis API from AYLIEN . This content is then fed to the Text-to-Speech API and it will read the article for you. The app can be extended to read the articles in different locale as well as different language. The complete list of supported locales and voice fonts can be found [here.](https://docs.microsoft.com/en-us/azure/cognitive-services/speech/api-reference-rest/bingvoiceoutput#SupLocales)

## Source Code
The entire source code is available here - [Code](/src/ArticleReader). To run the app locally, add the subscription key for Text-To-Speech API from Microsoft &, the app ID & key from [AYLIEN](https://developer.aylien.com/) in the web.config.