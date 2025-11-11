# VRC台球-排行榜-斯诺克
## 项目继承表
* #### MS-VRCSA-Billiards
  * #### VRChat-Pool-table-15-red-snooker-Pyramid-Chinese-8-MS-VRCSA-Billiards [地标更新](https://github.com/WangQAQ/VRChat-Pool-table-15-red-snooker-Pyramid-Chinese-8-MS-VRCSA-Billiards)
  	  * ### VRChat-Pool-table-Leaderboard-MS-VRCSA-Billiards (当前)
### 该桌子的目的是为大家带来一个面向娱乐的桌子,做一些小插件,或者好玩滴功能
#### [版权表](https://github.com/WangQAQ/VRChat-Pool-table-Leaderboard-MS-VRCSA-Billiards/blob/main/Copyright.md)

> 您可以在这里游玩该桌子:[地图](https://vrchat.com/home/world/wrld_d9ac19bc-a8c4-42cd-b712-c66dd813bd8c/info)
---
## 1.新的功能
* ### 上传面板:
	* #### 无需再向以前一样将绑定的工作放到 Start 函数,当前的绑定在 EditTime 完成,无需更多的性能消耗
	* #### 简便的上传方式,您可以不必在意如何绑定,只需要在面板上按照顺序点击,即可完成绑定工作
	* #### 更好的排行榜密钥管理系统,我们注意到大家在使用自动化排行榜时,容易因为一些操作导致丢失密钥,现在我们制作了一个新的用户系统,让您可以绑定您的密钥
* ### 用户系统:
	* #### 您可以在[账号](https://www.wangqaq.com/PoolBar/Account)中注册您的账号
	* #### 您现在可以在 WEB 上为自己选择彩名,只要注册了都能选喵
	* #### 尽管现在也可以匿名上传密钥,但是为了防止大家在后续丢失密钥导致无法使用排行榜,推荐注册一个账号(里面还能累计经验)
	* #### 您现在可以在 WEB 上选择自己的地标
* ### 新的优化:
	* #### 我修复了曾经排行榜的 GC 问题,现在不会有向以前一样上万次的GC了
	* #### 我优化了地标的 Shader,现在不会应为地标卡顿了
	* #### 我更新了彩名的方式,现在是 WEB 创建完成完整彩名后缓存,这样就不会在UDON中有多次循环了
  	* #### 新的面板用来绑定脚本，无需再 Start 占用游戏加载时间
* ### 新的福利:
	* #### 彩名现在无限制,您可以注册账号并绑定玩家,然后选择自己的彩名
	* #### 地标现在上传地图,会给大家发 3 个卡通地标喵 (自动滴没做完，大家可以在 QQ 群里面叫我,或者发个 Issues 附带注册邮箱喵)
	* #### 等级系统 ( 12 级 2 段彩名,24级 3 段彩名, 36级 4段彩名 )(没做完,过几天应该就有了)
* ### 新的 WEB:
	* #### 添加了个人面板,可以给自己设置地标和彩名
	* #### 添加了个人信息面板,查看自己上传次数和历史记录
	* #### 添加了排行榜记录功能,现在能够帮你计算胜率
	* #### 靠胜率做了个简易的分段,这样可以让大家不止注重分数
---

## 2.我该如何使用它
### 打开 Prefab 文件夹 ( 红色为必选，蓝色为可选 )
![1](https://github.com/user-attachments/assets/24566164-7c7a-4d29-b29f-d012d887821e)
* #### 放置 snooker&pyramid&cn8&3c&10b (主体台球桌)
* #### 放置 TableHook (replica) 2 (必备插件)
* #### 放置 UI-排行榜 (排行榜UI)
* #### 放置 TagPlug (地标主体) (选装插件)

## 然后在上方找到 VRC-VRCSA 
![1](https://github.com/user-attachments/assets/09701d17-b73e-4cee-b834-ca5cb6385cdd)
* ### 先按 Set Up Pool Table Layers
* ### 再打开 Build Tool
	* #### 登录您的账号(可选)
	* #### 按顺序从上往下点击按钮
 	* #### 早期测试版，可能有一些BUG	

## Web系统使用方式 (早期测试)
* ### 在[个人主页](https://www.wangqaq.com/PoolBar/Account)中可以做的
	* #### 里面可以设置您的彩名
	* #### 里面可以设置您的地标
	* #### 若您想要使用彩名的话,需要先绑定用户,前往[地图](https://vrchat.com/home/world/wrld_d9ac19bc-a8c4-42cd-b712-c66dd813bd8c/info)找到User Code System 复制里面的用户代码到 WEB,然后按绑定即可
 	*  ![1](https://github.com/user-attachments/assets/b2f3a365-6ebe-452e-9d75-8b798ee98ac2)
* ### 在[信息板](https://www.wangqaq.com/PoolBar/Information)中可以做的
	* #### 里面可以看到您的赛季统计和用户等级,任务表,活动等等信息

## 更新计划
	* #### 排行榜历史没有完全搭建完毕，完整的系统在后天完成
	* #### 计分器小修（下个小版本）
	* #### WebGL 台球历史回合查看
 	* #### 更好的上传链接更新
  	* #### 抽象台球小游戏

## 特别感谢

### 非常感谢大家，呜呜呜————(排名不分先后):COCO , 蛋包饭 , 安洁罗塞塔,猪排,里爱吃,等等
### 特别感谢COCO大人，为我们做了logo,前端，UI（UI和前端因为时间原因晚点会上传到项目）

## 高清大图
![1](https://github.com/user-attachments/assets/22d982b4-a50e-420f-8db5-05553483445d)
![1](https://github.com/user-attachments/assets/3ab92dda-c7dc-4ab1-94dd-bce85f6809e2)
![1](https://github.com/user-attachments/assets/90a37503-a4c4-4b7f-936c-17f00c094bec)

![qrcode_1737098291587](https://github.com/user-attachments/assets/ebbfe76c-75b4-4352-b105-5e02ae20ff09)
> 时间紧凑，双语版的页面在做了 </br>
> Time was tight, and the bilingual version of the page was done

