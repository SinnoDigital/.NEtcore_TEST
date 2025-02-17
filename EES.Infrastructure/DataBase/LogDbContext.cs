﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using EES.Infrastructure.Log;
using Microsoft.EntityFrameworkCore;

namespace EES.Infrastructure.DataBase;

public partial class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options)
        : base(options)
    {

    }

    public virtual DbSet<ApiLog> ApiLogs { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ApiLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_log__3213E83FB2C3BE0A");

            entity.ToTable("api_log", tb => tb.HasComment("接口请求记录表"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiRoute)
                
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("接口名")
                .HasColumnName("api_route");
            entity.Property(e => e.CreateTime)
                .HasComment("数据记录时间")
                .HasColumnName("create_time");
            entity.Property(e => e.Platform)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("登录平台")
                .HasColumnName("platform");
            entity.Property(e => e.RequestBody)
                
                .IsUnicode(false)
                .HasComment("请求参数的body")
                .HasColumnName("request_body");
            entity.Property(e => e.RequestHeader)
                
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasComment("请求的header数据")
                .HasColumnName("request_header");
            entity.Property(e => e.RequestQuery)
                
                .IsUnicode(false)
                .HasComment("请求参数的querystring")
                .HasColumnName("request_query");
            entity.Property(e => e.RequestTime)
                .HasComment("请求时间")
                .HasColumnName("request_time");
            entity.Property(e => e.RequestType)
                
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasComment("接口请求类型   post  或者get")
                .HasColumnName("request_type");
            entity.Property(e => e.RequestUrl)
                
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasComment("请求url")
                .HasColumnName("request_url");
            entity.Property(e => e.Response)
                
                .IsUnicode(false)
                .HasComment("响应参数")
                .HasColumnName("response");
            entity.Property(e => e.TimeConsumption)
                .HasComment("总计耗时（毫秒）")
                .HasColumnName("time_consumption");
            entity.Property(e => e.TraceId)
                
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("trace_id")
                .HasColumnName("trace_id");
            entity.Property(e => e.UserId)
                .HasComment("用户id")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audit_lo__3213E83FBFAEE317");

            entity.ToTable("audit_log", tb => tb.HasComment("数据审计表- 仅记录修改和删除"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasComment("数据记录时间")
                .HasColumnName("create_time");
            entity.Property(e => e.DbName)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("数据库名")
                .HasColumnName("db_name");
            entity.Property(e => e.EntityName)
                
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("业务实体名")
                .HasColumnName("entity_name");
            entity.Property(e => e.ObjectId)
                .HasComment("对象主键id")
                .HasColumnName("object_id");
            entity.Property(e => e.OperateTime)
                .HasComment("操作时间")
                .HasColumnName("operate_time");
            entity.Property(e => e.OperateType)
                .HasComment("操作类型： 1 修改    -1 删除")
                .HasColumnName("operate_type");
            entity.Property(e => e.OperatorId)
                .HasComment("操作人")
                .HasColumnName("operator_id");
            entity.Property(e => e.OperatorName)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("操作人名称")
                .HasColumnName("operator_name");
            entity.Property(e => e.OriginalValue)
                
                .IsUnicode(false)
                .HasComment("修改之前的值，json")
                .HasColumnName("original_value");
            entity.Property(e => e.SetValue)
                
                .IsUnicode(false)
                .HasComment("修改之后的值 , json")
                .HasColumnName("set_value");
            entity.Property(e => e.TableName)
                
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("表名")
                .HasColumnName("table_name");
        });

        modelBuilder.Entity<ExceptionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exceptio__3213E83F102472A8");

            entity.ToTable("exception_log", tb => tb.HasComment("异常信息记录表"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasComment("数据记录时间")
                .HasColumnName("create_time");
            entity.Property(e => e.ExceptionType)
                
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("异常类型")
                .HasColumnName("exception_type");
            entity.Property(e => e.Flag)
                
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("异常标识")
                .HasColumnName("flag");
            entity.Property(e => e.Message)
                
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("异常提示信息")
                .HasColumnName("message");
            entity.Property(e => e.OccurredTime)
                .HasComment("异常发生的时间")
                .HasColumnName("occurred_time");
            entity.Property(e => e.StackTrace)
                
                .IsUnicode(false)
                .HasComment("堆栈信息")
                .HasColumnName("stack_trace");
            entity.Property(e => e.Text)
                
                .IsUnicode(false)
                .HasComment("异常的完整错误信息")
                .HasColumnName("text");
            entity.Property(e => e.TraceId)
                
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("trace_id")
                .HasColumnName("trace_id");
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__login_lo__3213E83FE3F8A447");

            entity.ToTable("login_log", tb => tb.HasComment("用户登录记录表"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasComment("数据记录时间")
                .HasColumnName("create_time");
            entity.Property(e => e.IpV4)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("ip")
                .HasColumnName("ip_v4");
            entity.Property(e => e.IpV6)
                
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("v6ip  暂时预留")
                .HasColumnName("ip_v6");
            entity.Property(e => e.LoginPlatform)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("登录平台")
                .HasColumnName("login_platform");
            entity.Property(e => e.LoginTime)
                .HasComment("登陆时间")
                .HasColumnName("login_time");
            entity.Property(e => e.Token)
                
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("授权token")
                .HasColumnName("token");
            entity.Property(e => e.UserAccount)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("登录用户名")
                .HasColumnName("user_account");
            entity.Property(e => e.UserAgent)
                
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasComment("header里的UA标识")
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId)
                .HasComment("用户id")
                .HasColumnName("user_id");
            entity.Property(e => e.UserName)
                
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("用户名称")
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__operatio__3213E83F871411C9");

            entity.ToTable("operation_log", tb => tb.HasComment("用户操作记录表"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchId)
                
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("批次号")
                .HasColumnName("batch_id");
            entity.Property(e => e.CreateTime)
                .HasComment("数据记录时间")
                .HasColumnName("create_time");
            entity.Property(e => e.ModuleFlag)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("模块标识")
                .HasColumnName("module_flag");
            entity.Property(e => e.ModuleName)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("模块名字")
                .HasColumnName("module_name");
            entity.Property(e => e.ObjectId)
                .HasComment("操作对象的id")
                .HasColumnName("object_id");
            entity.Property(e => e.ObjectKeywords)
                
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasComment("关键字信息，比如 工单号， 仓库编号等")
                .HasColumnName("object_keywords");
            entity.Property(e => e.OperateTime)
                .HasComment("操作时间")
                .HasColumnName("operate_time");
            entity.Property(e => e.OperateType)
                .HasComment("操作类型 新增，修改，删除，审批，等，具体再定义")
                .HasColumnName("operate_type");
            entity.Property(e => e.OperateTypeDesc)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("操作类型的文字说明")
                .HasColumnName("operate_type_desc");
            entity.Property(e => e.Remark)
                
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.SubModuleFlag)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("子模块标识")
                .HasColumnName("sub_module_flag");
            entity.Property(e => e.SubModuleName)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("子模块名字")
                .HasColumnName("sub_module_name");
            entity.Property(e => e.UserId)
                .HasComment("用户id")
                .HasColumnName("user_id");
            entity.Property(e => e.UserName)
                
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("用户名称")
                .HasColumnName("user_name");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}