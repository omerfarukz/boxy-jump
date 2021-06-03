// save this code at //Assets/Plugins/iOS/omrlib.m
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

typedef void (* UnityCommandCallback)(const char * objectName, const char * commandName, const char * commandData);

// you can create different file named omrlib.h but it is not necessary. demo is simple.
@interface omrlib : NSObject

- (void) showMessage        :   (NSString *)    title
         secondValue        :   (NSString *)    message
         thirdValue         :   (NSString *)    objectName
         fourthValue        :   (NSString *)    commandName;

- (void) showShareDialog    :   (NSString *)    text
         secondValue        :   (NSString *)    url
         thirdValue         :   (NSString *)    imagepath;

- (void) connectCallback    :   (UnityCommandCallback)  callbackMethod;

- (void) showRateUs         :   (NSString *)    url
         secondValue        :   (NSString *)    title
         thirdValue         :   (NSString *)    message
         fourthValue        :   (NSString *)    yesTitle
         fifthValue         :   (NSString *)    noTitle;

- (void) callMethod         :   (const char *)  objectName
         secondValue        :   (const char *)  commandName
         thirdValue         :   (const char *)  commandData;

@end

@implementation omrlib

static UnityCommandCallback lastCallBack = NULL;

- (void) showMessage        :   (NSString *)    title
         secondValue        :   (NSString *)    message
         thirdValue         :   (NSString *)    objectName
         fourthValue        :   (NSString *)    commandName
{
    UIAlertController * alert =   [UIAlertController
                                  alertControllerWithTitle:title
                                  message:message
                                  preferredStyle:UIAlertViewStyleDefault];
    
    UIAlertAction* ok = [UIAlertAction
                         actionWithTitle:@"OK"
                         style:UIAlertActionStyleDefault
                         handler:^(UIAlertAction * action)
                         {
                             [alert dismissViewControllerAnimated:YES completion:nil];
                             
                             [self callMethod:[objectName cStringUsingEncoding:NSUTF8StringEncoding]
                                  secondValue:[commandName cStringUsingEncoding:NSUTF8StringEncoding]
                                   thirdValue:[@"OK" cStringUsingEncoding:NSUTF8StringEncoding]];
                         }];
    
    UIAlertAction* cancel = [UIAlertAction
                             actionWithTitle:@"Cancel"
                             style:UIAlertActionStyleCancel
                             handler:^(UIAlertAction * action)
                             {
                                 [alert dismissViewControllerAnimated:YES completion:nil];
                                 [self callMethod:[objectName cStringUsingEncoding:NSUTF8StringEncoding]
                                      secondValue:[commandName cStringUsingEncoding:NSUTF8StringEncoding]
                                       thirdValue:[@"CANCEL" cStringUsingEncoding:NSUTF8StringEncoding]];
                             }];
    
    [alert addAction:ok];
    [alert addAction:cancel];
    
    [UnityGetGLViewController() presentViewController:alert animated:YES completion:nil];
}

- (void) showShareDialog    :   (NSString *)    text
         secondValue        :   (NSString *)    url
         thirdValue         :   (NSString *)    imagepath
{
    UIImage *img = [[UIImage alloc] initWithContentsOfFile:imagepath];
    NSArray * activityItems = @[img, text, [NSURL URLWithString:url]];
    NSArray * applicationActivities = nil;
    NSArray * excludeActivities = @[UIActivityTypeAssignToContact, UIActivityTypePostToWeibo, UIActivityTypePrint];
    
    UIActivityViewController * activityController = [[UIActivityViewController alloc] initWithActivityItems:activityItems applicationActivities:applicationActivities];
    activityController.excludedActivityTypes = excludeActivities;
    
    [UnityGetGLViewController() presentViewController:activityController animated:YES completion:nil];
}

- (void) showRateUs         :   (NSString *)    url
         secondValue        :   (NSString *)    title
         thirdValue         :   (NSString *)    message
         fourthValue        :   (NSString *)    yesTitle
         fifthValue         :   (NSString *)    noTitle
{
    UIAlertController * alert =   [UIAlertController
                                   alertControllerWithTitle:title
                                   message:message
                                   preferredStyle:UIAlertControllerStyleAlert];
    
    UIAlertAction* yes = [UIAlertAction
                         actionWithTitle:yesTitle
                         style:UIAlertActionStyleDefault
                         handler:^(UIAlertAction * action)
                         {
                             [alert dismissViewControllerAnimated:YES completion:nil];
                             [[UIApplication sharedApplication] openURL:[NSURL URLWithString: url]];
                         }];
    
    UIAlertAction* no = [UIAlertAction
                             actionWithTitle:noTitle
                             style:UIAlertActionStyleDestructive
                             handler:^(UIAlertAction * action)
                             {
                                 [alert dismissViewControllerAnimated:YES completion:nil];
                             }];
    
    [alert addAction:no];
    [alert addAction:yes];

    
    [UnityGetGLViewController() presentViewController:alert animated:YES completion:nil];
}


- (void) connectCallback    :   (UnityCommandCallback)  callbackMethod
{
    lastCallBack = callbackMethod;
}

- (void) callMethod         :   (const char *)  objectName
         secondValue        :   (const char *)  commandName
         thirdValue         :   (const char *)  commandData
{
    if(lastCallBack != NULL)
    {
        lastCallBack(objectName, commandName, commandData);
    }
}
@end


// extern methods
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

extern UIViewController *UnityGetGLViewController(); // Root view controller of Unity screen.

extern void _showMessage(
                         const char *   title       ,
                         const char *   message     ,
                         const char *   objectName  ,
                         const char *   commandName
                        )
{
    omrlib* mylib = [[omrlib alloc] init];
    [mylib showMessage  :   GetStringParam(title)
            secondValue :   GetStringParam(message)
            thirdValue  :   GetStringParam(objectName)
            fourthValue :   GetStringParam(commandName)
    ];
}
extern void _showShareDialog(
                             const char *    text    ,
                             const char *    url     ,
                             const char *    imagepath
                            )
{
    omrlib* mylib = [[omrlib alloc] init];
    [mylib showShareDialog:GetStringParam(text) secondValue:GetStringParam(url) thirdValue:GetStringParam(imagepath)];
}

extern void _showRateUs(
                        const char *   url          ,
                        const char *   title        ,
                        const char *   message      ,
                        const char *   yesTitle     ,
                        const char *   noTitle
                       )
{
    omrlib* mylib = [[omrlib alloc] init];
    [mylib showRateUs       :   GetStringParam(url)
            secondValue     :   GetStringParam(title)
            thirdValue      :   GetStringParam(message)
            fourthValue     :   GetStringParam(yesTitle)
            fifthValue      :   GetStringParam(noTitle)
    ];
}


extern void _connectCallback(UnityCommandCallback callbackname) {
    omrlib* mylib = [[omrlib alloc] init];
    [mylib connectCallback:callbackname];
}

extern void _callMethod(const char * objectName, const char * commandName, const char * commandData) {
    omrlib* mylib = [[omrlib alloc] init];
    [mylib callMethod:objectName secondValue:commandName thirdValue:commandData];
}