﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Framework.Domain;
using Framework.Helper;
using System.IO;
using Framework.Validate;
using Framework.DTO;
using Framework.BLL;
using Framework.DI;

namespace Framework.web.Controllers
{
    public class APISuperController : Controller
    {
        //响应的对象封装对象
        protected ResponseData result = new ResponseData
        {
            Data = null,
            Succeeded = false,
            Msg = ""
        };

        //ajax请求的参数封装对象
        protected RequestData RequestParameters = new RequestData
        {
            data = null,
            identity = ""
        };

        /// <summary>
        /// 会话记录的用户
        /// </summary>
        protected EHECD_SystemUserDTO SessionUser;

        /// <summary>
        /// 处理前
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                if (requestContext.HttpContext.Request.IsAjaxRequest() && requestContext.HttpContext.Request.RequestType.ToLower() == "post")
                {
                    //预备给脚本端的数据获取接口
                    RequestParameters = ParameterLoader.LoadAjaxPostParameters(requestContext.HttpContext.Request.InputStream);
                }
                else
                {
                    //直接post的加密接口
                    //RequestParameters = ParameterLoader.LoadSecurityParameters(requestContext.HttpContext.Request.InputStream);

                    /**
                     * 载入客户端post上来的请求
                     * */
                    RequestParameters = ParameterLoader.LoadAjaxPostParameters(requestContext.HttpContext.Request.InputStream);

                    //如果没有上传identity参数，表示客户端还没有登录或者客户端觉得不需要登录，则不初始化session user
                    if (!string.IsNullOrEmpty(RequestParameters.identity) && !string.IsNullOrWhiteSpace(RequestParameters.identity))
                    {
                        //保存在Session中
                        SessionInfo session = (SessionInfo)Session[SessionInfo.APISESSION_NAME];

                        //session里面有用户信息
                        if (session != null && session.SessionUser != null && session.SessionUser.User != null)
                        {
                            //如果传入identity不一致，以客户端为准
                            if (session.SessionUser.User.ID.ToString() != RequestParameters.identity)
                            {
                                //获取用户并保存到session
                                ILogin login = DIEntity.GetInstance().GetImpl<ILogin>();
                                session.SessionUser.User = login.GetAppLoginInfo(RequestParameters.identity);
                                Session[SessionInfo.APISESSION_NAME] = session;
                                SessionUser = session.SessionUser.User;
                            }
                            else
                            {
                                SessionUser = session.SessionUser.User;
                            }
                        }
                        else
                        {
                            ILogin login = DIEntity.GetInstance().GetImpl<ILogin>();
                            SessionInfo info = new SessionInfo();
                            info.SessionUser.User = login.GetAppLoginInfo(RequestParameters.identity);
                            Session[SessionInfo.APISESSION_NAME] = session;
                            SessionUser = session.SessionUser.User;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SystemLog.Logs.GetLog().WriteErrorLog(e);
            }
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 处理后
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != default(Exception))
            {
                if (filterContext.Exception.GetType() == typeof(DomainInfoException))
                {
                    var ex = filterContext.Exception as DomainInfoException;
                    filterContext.ExceptionHandled = true;
                    if (ex.IsLog)
                    {
                        SystemLog.Logs.GetLog().WriteErrorLog(ex);
                        result.Succeeded = false;
                        result.Msg = ex.Message;
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Msg = ex.Message;
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                }
                else
                {
                    //处理controller的异常
                    SystemLog.Logs.GetLog().WriteErrorLog(filterContext.Exception);
                    filterContext.ExceptionHandled = true;
                }
            }
            else //if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.RequestType.ToLower() == "post")
            {
                //filterContext.Result = File(Security.EncryptionResponse(ParameterLoader.LoadResponseJSONStr(result)), "application/octet-stream");
                //var ret = Security.EncryptionResponse(ParameterLoader.LoadResponseJSONStr(result));                
                //filterContext.HttpContext.Response.BinaryWrite(ret);
                filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
            }
        }
    }
}