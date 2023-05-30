// openyoureye.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"

#using <mscorlib.dll>
#using <System.Dll>
#using <System.Data.Dll>
#using <System.Xml.Dll>


using namespace System;
using namespace System::Data;
using namespace System::Xml;
using namespace System::Collections;
using namespace System::Data::SqlClient;
//---臉眼睛宣告
static CvMemStorage* storage = 0;
static CvHaarClassifierCascade* cascade = 0;
void detect_and_draw( IplImage* image );
int face(const char* filename);
const char* cascade_name;
//轉直判斷
int HistogramBins = 2;
float HistogramRange1[]={0,256};
float *HistogramRange[1]={&HistogramRange1[0]};
double scale = 1.3;
//-------------

float eyeval,dtl,aq;   //樣本變數
int open,close,radius,radiuss,qq=0,tt=0,bk=0,rr,oc=0;    //張眼閉眼變數半徑眨眼周其次數周期平均
void notval();//轉值
void dataadd(float ddd);//資料庫存入
void eye();
//==字串轉換結合
void mystrncat(char *ptr1,char *ptr2);
char * userid;
static char s1[200];
int main(int argv ,char *argc[])
{

	SqlConnection* objConn;//新增開始
    try
    {
        String* sConnectionString;
        sConnectionString = "Password=open16CV;User ID=sa;Initial Catalog=openeyes;Data Source=127.0.0.1";
        objConn = new SqlConnection(sConnectionString);
        objConn->Open();
	}
    catch(Exception *ex)
    {
    }
        
    userid=argc[1];
	face("picture\\sample.jpg");
	SqlCommand * objcomm=objConn->CreateCommand();
	objcomm->CommandText=s1;
	objcomm->ExecuteNonQuery();
	for (;;)
	{
		char s3[200]="picture\\";
		char s4[]=".jpg";
		char nono[25];
		itoa(tt,nono,10);
		mystrncat(s3,nono);
		mystrncat(s3,s4);
		int xxs=face(s3);
		if (xxs==0){
		SqlCommand * objcomm=objConn->CreateCommand();
		objcomm->CommandText=s1;
		objcomm->ExecuteNonQuery();}
		if (xxs>50){break;}
	}
	objConn->Close();
	return 0;
} 
int face(const char* filename)	//抓臉
{
		IplImage * image = cvLoadImage( filename, 1 );
		cascade_name = "picture\\haarcascade_frontalface_alt.xml";
		cascade = (CvHaarClassifierCascade*)cvLoad( cascade_name, 0, 0, 0 );
		storage = cvCreateMemStorage(0);
        if( image )
        {
			if(tt<9){tt++;}else{tt=0;}
            detect_and_draw( image );
            cvReleaseImage( &image );
			
			qq=0;
        }
        else
        {
			qq++;
        }
		cvClearMemStorage( storage );
		return qq;
}

void detect_and_draw( IplImage* img )  //畫圈圈
{ 
   IplImage* gray = cvCreateImage( cvSize(img->width,img->height), 8, 1 );
   IplImage* small_img = cvCreateImage( cvSize( cvRound (img->width/scale),
                         cvRound (img->height/scale)),
						 8, 1 );
    int i;
	int smp=0; //旗桿判斷照片是否有抓臉
    cvCvtColor( img, gray, CV_BGR2GRAY );
    cvResize( gray, small_img, CV_INTER_LINEAR );
    cvEqualizeHist( small_img, small_img );
    cvClearMemStorage( storage );
	CvSeq* faces = cvHaarDetectObjects( small_img, cascade, storage,1.1, 2, 0,cvSize(30, 30) );
	if ((faces ? faces->total : 0)>0)
	{
		CvRect* r = (CvRect*)cvGetSeqElem( faces, (faces ? faces->total : 0)-1 );
		CvPoint center;
		center.x = cvRound((r->x + r->width*0.5)*scale);
		center.y = cvRound((r->y + r->height*0.5)*scale);
		radius = cvRound((r->width + r->height)*0.25*scale);
		if (eyeval==0) rr=radius;
		if (radius>=rr*0.9 || radius<=rr*1.1) {
		IplImage* newimg=img;     //以下存眼睛圖
		CvRect Rect1=cvRect(center.x-radius,center.y-radius,radius*2,radius);
		cvSetImageROI(newimg,Rect1);
		cvSaveImage("picture\\aa.jpg",newimg);//到此存完圖
		cvResetImageROI(newimg);
		eye();}else{bk++;}
	}else{bk++;}
	if (bk==2){dataadd(-3);bk=0;}
	cvReleaseImage( &gray );
    cvReleaseImage( &small_img );
}

void eye()
{
	IplImage* img=cvLoadImage("picture\\aa.jpg", 1 );
    IplImage* gray = cvCreateImage( cvSize(img->width,img->height), 8, 1 );
	IplImage* small_img = cvCreateImage( cvSize( cvRound (img->width/scale),
                          cvRound (img->height/scale)),
						  8, 1 );

		cascade_name = "picture\\haarcascade_righteye_2splits.xml";
		cascade = (CvHaarClassifierCascade*)cvLoad( cascade_name, 0, 0, 0 );
		storage = cvCreateMemStorage(0);
		cvCvtColor(img, gray, CV_BGR2GRAY );
		cvResize( gray, small_img, CV_INTER_LINEAR );
		cvEqualizeHist( small_img, small_img );
		cvClearMemStorage( storage );
        CvPoint center;
		CvSeq* faces = cvHaarDetectObjects( small_img, cascade, storage,1.1, 2, 0,cvSize(30, 30) );
		if ((faces ? faces->total : 0)>0)
		{
			for (int i=0;i<(faces ? faces->total : 0);i++)
			{
				CvRect* r = (CvRect*)cvGetSeqElem( faces, i );
				center.x = cvRound((r->x + r->width*0.5)*scale);
			if (center.x<(img->width/2))
			{
				if (eyeval==0){radiuss = cvRound((r->width + r->height)*0.25*scale);}
				center.y = cvRound((r->y + r->height*0.5)*scale);
				IplImage* newimg=img;     //以下存眼睛圖
				CvRect Rect1=cvRect(center.x-radiuss*0.4,center.y,radiuss*1.1,radiuss*0.37);
				//CvRect Rect1=cvRect(center.x-radiuss*0.7,center.y-radiuss*0.29,radiuss*1.1,radiuss*0.71);
				cvSetImageROI(newimg,Rect1);
				cvSaveImage("picture\\bb.jpg",newimg);//到此存完圖
				cvResetImageROI(newimg);
				notval();
				break;
			}
			if (i+1==(faces ? faces->total : 0))
			{
				open=0;
				close=close+1;
				dtl=(float)close/-3;
				dataadd(dtl);
			}
			}
		}else{
				open=0;
				close=close+1;
				dtl=(float)close/-3;
				dataadd(dtl);
		}
	cvReleaseImage( &img);
	cvReleaseImage( &gray );
    cvReleaseImage( &small_img );
}

void notval()//----轉直
{
		//-------------二直
	IplImage *Image1;//存放原始圖檔
    float newval;

	Image1 = cvLoadImage("picture\\bb.jpg",CV_LOAD_IMAGE_GRAYSCALE);//讀取照片

	//===濾波器
		IplImage *Image3;
		Image3=Image1;
		cvSmooth(Image1, Image3,CV_MEDIAN ,3, 3, 1, 0 );
	 // ---------值方
    CvHistogram *Histogram1;
    Histogram1 = cvCreateHist(1,&HistogramBins,CV_HIST_ARRAY,HistogramRange);
	float sum=0; //圖片黑白總合
	
	IplImage *Image2;//存放原始圖檔
	Image2=Image3;
	cvAdaptiveThreshold(Image3,Image2,256,0,0,13,5);	
    cvCalcHist(&Image2,Histogram1);
			sum=((CvMatND *) Histogram1->bins)->data.fl[0]+((CvMatND *) Histogram1->bins)->data.fl[1];

			newval =((CvMatND *) Histogram1->bins)->data.fl[0]/sum;

			cvReleaseHist(&Histogram1);
		
		float aq=(100-(newval*100))/100;
		aq=aq*1.05;
		if ((newval*100)<10){aq=0.9;}

		

		

			if (eyeval<newval)
			{
				open=open+1;
				close=0;
				dtl=(float)open/13;
				eyeval=newval*aq;			//存入樣本值
			}else{
				if (open<=2 && close==0){oc++;}else{oc=0;}
				open=0;
				close=close+1;
				dtl=(float)close/-4;
			}
			if (open>=2) {oc=0;}

			if (oc>=2){dtl=3;}


		dataadd(dtl);
		

}
void dataadd(float ddd)//資料庫存入
{
	if (ddd>5){ddd=5;}
	if (ddd<-5){ddd=-5;}
	for (int s=0;s<200;s++)
	{s1[s]='\0';}
	mystrncat(s1,"insert into dbo.行車紀錄(員工編號,危險指數,狀態) values('");
	char qqw[15];
	mystrncat(s1,userid);
	mystrncat(s1,"',");
	int aaw[]={ddd,((ddd*100000)-((int)ddd*100000))};   //切割整數根小數
	itoa(abs(aaw[0]),qqw,10);						//整數轉字串
	if (ddd<0){mystrncat(s1,"-");}
	mystrncat(s1,qqw);							//整數部分寫入S1
	mystrncat(s1,".");								//加小數點
	itoa(abs(aaw[1]),qqw,10);							//小數
	mystrncat(s1,qqw);
	switch (abs((int)ddd))
	{
		case 0:
			mystrncat(s1,",'正常')");
			break;
		case 1:
			mystrncat(s1,",'警告')");
			break;
		case 2:	
			mystrncat(s1,",'危險')");
			break;
		default:
			mystrncat(s1,",'嚴重')");
	}
}

void mystrncat(char *ptr1,char *ptr2) 
{

	while (*ptr1 != '\0')               // 跑到字串s1的尾巴

	{

		ptr1++;

	}

	while (*ptr2 != '\0')           // 將字串s2的值丟到字串中,直到N次
	{

		*ptr1 = *ptr2;

		ptr2++;

		ptr1++;

	}
}


