/*
 *  SSL client demonstration program
 *
 *  Copyright (C) 2006-2015, ARM Limited, All Rights Reserved
 *  SPDX-License-Identifier: Apache-2.0
 *
 *  Licensed under the Apache License, Version 2.0 (the "License"); you may
 *  not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 *  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  This file is part of mbed TLS (https://tls.mbed.org)
 */

#include MBEDTLS_CONFIG_FILE
#include "mbedtls/platform.h"
#include "lwip.h"
#include "lwip/api.h"

extern struct netif gnetif;

#include "ssl_client.h"
#include "usart.h"

#if defined(MBEDTLS_MEMORY_BUFFER_ALLOC_C)
#include "mbedtls/memory_buffer_alloc.h"
#endif

#include "mbedtls/net_sockets.h"
#include "mbedtls/debug.h"
#include "mbedtls/ssl.h"
#include "mbedtls/entropy.h"
#include "mbedtls/ctr_drbg.h"
#include "mbedtls/error.h"
#include "mbedtls/certs.h"

#include <string.h>

#define SERVER_PORT "5555"//"443"
#define SERVER_NAME "169.254.24.21"

/*
- "Country Name (2 letter code)" - PL
- "State or Province Name (full name)" - Malopolska
- "Locality Name (eg, city)" - Krakow
- "Organization Name (eg, company)" – HSK Data
- "Organizational Unit Name (eg, section)" -
- "Common Name (e.g. server FQDN or YOUR name)" - rcpkd
- "Email Address".
*/
#define DEBUG_LEVEL 1

#if 0
const char mbedtls_client_certificate[] =
	"-----BEGIN CERTIFICATE-----\r\n"
	"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\r\n"
	"NIrzbeAz1g==\r\n"
	"-----END CERTIFICATE-----\r\n";

const char mbedtls_certificate_key_pass[] = "test5432!";

const char mbedtls_certificate_key[] =
		"-----BEGIN ENCRYPTED PRIVATE KEY-----\r\n"
		"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\r\n"
		"5e/ibghY4Lc8gQtcLXtOKQ==\r\n"
		"-----END ENCRYPTED PRIVATE KEY-----\r\n"
#endif

const char mbedtls_client_certificate[] =
		"-----BEGIN CERTIFICATE-----\r\n"
		"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\r\n"
		"NIrzbeAz1g==\r\n"
		"-----END CERTIFICATE-----\r\n";

const char mbedtls_certificate_key_pass[] = "test5432!"; //cH$k102d@t@";

const char mbedtls_certificate_key[] =
		"-----BEGIN ENCRYPTED PRIVATE KEY-----\r\n"
		"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\r\n"
		"5e/ibghY4Lc8gQtcLXtOKQ==\r\n"
		"-----END ENCRYPTED PRIVATE KEY-----\r\n";

const size_t mbedtls_client_certificate_len = strlen(mbedtls_client_certificate);
const size_t mbedtls_certificate_key_len = sizeof(mbedtls_certificate_key);
const size_t mbedtls_certificate_key_pass_len = strlen(mbedtls_certificate_key_pass);

mbedtls_net_context server_fd;
unsigned char buf[1024];
const char *pers = "ssl_client1";

mbedtls_entropy_context entropy;
mbedtls_ctr_drbg_context ctr_drbg;
mbedtls_ssl_context ssl;
mbedtls_ssl_config conf;
mbedtls_x509_crt cacert;
mbedtls_pk_context pkey;

static void my_debug(void *ctx, int level, const char *file, int line, const char *str)
{
  ((void) level);

  mbedtls_fprintf((FILE*) ctx, "%s:%04d: %s", file, line, str);
  fflush((FILE*) ctx);
}

static void Initialize_SessionData(void)
{
	  mbedtls_net_init(&server_fd);
	  mbedtls_ssl_init(&ssl);
	  mbedtls_ssl_config_init(&conf);
	  mbedtls_x509_crt_init(&cacert);
	  mbedtls_ctr_drbg_init(&ctr_drbg);
	  mbedtls_pk_init(&pkey);
	  mbedtls_entropy_init(&entropy);
}

static int Drbg_Seed(void)
{
	int opStatus = 0;
	if((opStatus = mbedtls_ctr_drbg_seed(&ctr_drbg, mbedtls_entropy_func, &entropy, (const unsigned char*) pers, strlen(pers))) != 0) {
	    mbedtls_printf(" failed\r\n   ! mbedtls_ctr_drbg_seed returned %d\r\n ", opStatus);
	    return opStatus;
	}
	return opStatus;
}

static int InitializeCertificate(void)
{
	int opStatus = 0;

	fflush( stdout);

	opStatus = mbedtls_pk_parse_key( &pkey,
			  (const unsigned char*)mbedtls_certificate_key,
			  mbedtls_certificate_key_len,
			  (const unsigned char*)mbedtls_certificate_key_pass,
			  mbedtls_certificate_key_pass_len);

	if(opStatus < 0) {
		mbedtls_printf("ERROR - mbedtls_pk_parse_key returned -0x%x\r\n \r\n ", (unsigned int) -opStatus);
		return opStatus;
	}

	opStatus = mbedtls_x509_crt_parse(&cacert, (const unsigned char*)mbedtls_client_certificate, mbedtls_client_certificate_len + 1);

	if(opStatus < 0) {
		mbedtls_printf("ERROR - mbedtls_x509_crt_parse returned -0x%x\r\n \r\n ", (unsigned int) -opStatus);
	    return opStatus;
	}

	opStatus = mbedtls_ssl_conf_own_cert(&conf, &cacert, &pkey);
    if(opStatus < 0) {
		  mbedtls_printf("ERROR - mbedtls_ssl_conf_own_cert returned -0x%x\r\n \r\n ", (unsigned int) -opStatus);
		  return opStatus;
	}
    return opStatus;
}

static void ValidateIpAddress(void)
{
	while(1)
	{
	    if(gnetif.ip_addr.addr == 0 || gnetif.netmask.addr == 0 || gnetif.gw.addr == 0) //system has no valid ip address
	    {
	      osDelay(1000);
	      continue;
	    }
	    else
	    {
	      printf("DHCP/Static IP O.K.\r\n ");
	      break;
	    }
	}
}

static void ConnectToServerTcp(void)
{
	int opStatus = 0;

    while(1)
    {
      if((opStatus = mbedtls_net_connect(&server_fd, SERVER_NAME, SERVER_PORT, MBEDTLS_NET_PROTO_TCP)) == 0)
      {
    	mbedtls_printf("mbedtls_net_connect OK\r\n ");
        break;
      }
      else
      {
        mbedtls_printf("ERROR - mbedtls_net_connect returned %d\r\n \r\n ", opStatus);
        osDelay(100);
      }
    }
}

static uint8_t LoadDefaultConfiguration(void)
{
	int opStatus = 0;

	 if((opStatus = mbedtls_ssl_config_defaults(&conf, MBEDTLS_SSL_IS_CLIENT, MBEDTLS_SSL_TRANSPORT_STREAM, MBEDTLS_SSL_PRESET_DEFAULT)) != 0)
    {
      mbedtls_printf("ERROR - mbedtls_ssl_config_defaults returned %d\r\n \r\n ", opStatus);
      return opStatus;
    }

    return opStatus;
}

static uint8_t ConnectionSSL_Configuration(void)
{
	int opStatus = 0;

    mbedtls_ssl_conf_authmode(&conf, MBEDTLS_SSL_VERIFY_OPTIONAL);
    mbedtls_ssl_conf_ca_chain(&conf, &cacert, NULL);
    mbedtls_ssl_conf_rng(&conf, mbedtls_ctr_drbg_random, &ctr_drbg);
    mbedtls_ssl_conf_dbg(&conf, my_debug, stdout);

    if((opStatus = mbedtls_ssl_setup(&ssl, &conf)) != 0)
    {
      mbedtls_printf("ERROR - mbedtls_ssl_setup returned %d\r\n \r\n ", opStatus);
      return opStatus;
    }

    return opStatus;
}

static uint8_t SetHostname_BioCallbacks(void)
{
	int opStatus = 0;

    if((opStatus = mbedtls_ssl_set_hostname(&ssl, "rcpkd")) != 0)
    {
      mbedtls_printf("ERROR - mbedtls_ssl_set_hostname returned %d\r\n \r\n ", opStatus);
      return opStatus;
    }

    mbedtls_ssl_set_bio(&ssl, &server_fd, mbedtls_net_send, mbedtls_net_recv, NULL);
    return opStatus;
}

static uint8_t Handshake(void)
{
	int opStatus = 0;

    while((opStatus = mbedtls_ssl_handshake(&ssl)) != 0)
    {
      if(opStatus != MBEDTLS_ERR_SSL_WANT_READ && opStatus != MBEDTLS_ERR_SSL_WANT_WRITE)
      {
        mbedtls_printf("ERROR - mbedtls_ssl_handshake returned -0x%x\r\n \r\n ", opStatus);
        return opStatus;
      }
    }

    return opStatus;
}


static void VarifyCertificate(void)
{
	/*
	 * When using VerifyCertificate when self sign certificate it will generate error.
	 */
#if 0
	uint32_t flags = 0x00;;
    if((flags = mbedtls_ssl_get_verify_result(&ssl)) != 0)
    {
      char vrfy_buf[512];
      mbedtls_printf(" failed\r\n ");
      mbedtls_x509_crt_verify_info(vrfy_buf, sizeof(vrfy_buf), "\r\n \r\n ", flags);
      mbedtls_printf("%s\r\n ", vrfy_buf);
    }
#endif
}

static int SendTestMsg(void)
{
	int opStatus = 0;
	int msgLenToSend = 0;

	//buf is global buffer
	msgLenToSend = sprintf((char*)buf, "testmsg,123,76\r");

    while((opStatus = mbedtls_ssl_write(&ssl, buf, msgLenToSend)) <= 0)
    {
      if(opStatus != MBEDTLS_ERR_SSL_WANT_READ && opStatus != MBEDTLS_ERR_SSL_WANT_WRITE)
      {
        mbedtls_printf("ERROR - mbedtls_ssl_write returned %d\n\n", opStatus);
        return -1;
      }
    }
    return msgLenToSend;
}

static int ReadWriteData(void)
{
	int msgLen = 0;
	int opStatus = 0;

	msgLen = sizeof(buf) - 1;
    memset(buf, 0, sizeof(buf));

    opStatus = mbedtls_ssl_read(&ssl, buf, msgLen);

    if(opStatus == MBEDTLS_ERR_SSL_WANT_READ || opStatus == MBEDTLS_ERR_SSL_WANT_WRITE) {
    	mbedtls_printf("write/read ok");
    	fflush( stdout);
    }
    if(opStatus == MBEDTLS_ERR_SSL_PEER_CLOSE_NOTIFY) {
    	return -1;
    }

    if(opStatus < 0) {
      mbedtls_printf("ERROR - mbedtls_ssl_read returned %d\r\n \r\n ", opStatus);
      return -1;
    }

    if(opStatus == 0) {
  	  mbedtls_printf("ret == 0 return %d\r\n \r\n ", opStatus);
  	  return -1;
    }

    if(opStatus > 0) {
    	msgLen = opStatus;
        mbedtls_printf(" %d bytes read\n\n %s", msgLen, (char*)buf);
        fflush( stdout);	//Bez fflush wysyla z bufora wczesniej odebrana wiadomosc.

        if(msgLen > 0) {
        	while((opStatus = mbedtls_ssl_write(&ssl, buf, msgLen)) <= 0)
			{
        		if(opStatus != MBEDTLS_ERR_SSL_WANT_READ && opStatus != MBEDTLS_ERR_SSL_WANT_WRITE)
				{
				  mbedtls_printf("ERROR - mbedtls_ssl_write returned %d\r\n \r\n ", opStatus);
				}
			}
        }
    }

    return 0;
}

static void ClearData(void)
{
	mbedtls_net_free(&server_fd);
	mbedtls_x509_crt_free(&cacert);
	mbedtls_ssl_free(&ssl);
	mbedtls_ssl_config_free(&conf);
	mbedtls_ctr_drbg_free(&ctr_drbg);
	mbedtls_entropy_free(&entropy);
}

void StartSSLClientTask(void const *argument)
{
	int ret = 1;

	#if defined(MBEDTLS_DEBUG_C)
	mbedtls_debug_set_threshold(DEBUG_LEVEL);
	#endif

	Initialize_SessionData();
	ret = Drbg_Seed();

	if(ret != 0) {
		mbedtls_printf("ERROR\r\n");
    	goto exit;
	}

	ret = InitializeCertificate();
	if(ret != 0) {
		mbedtls_printf("ERROR\r\n");
		goto exit;
	}

	ValidateIpAddress();

  while(1)
  {
    /*
     * 1. Start the connection
     */
    mbedtls_printf("  . Connecting to tcp/%s/%s...\r\n ", SERVER_NAME, SERVER_PORT);
    fflush( stdout);

    ConnectToServerTcp();

    mbedtls_printf("  . Setting up the SSL/TLS structure...\r\n ");
    fflush( stdout);

    if(LoadDefaultConfiguration() != 0) { goto exit; }
    if(ConnectionSSL_Configuration() != 0) { goto exit; }
    if(SetHostname_BioCallbacks() != 0) { goto exit; }
	SetHostname_BioCallbacks();

	if(Handshake() != 0) { goto exit; }

	VarifyCertificate();

    mbedtls_printf("\r\n Write data to server:\r\n ");
    fflush( stdout);

	if(SendTestMsg() < 0) { goto exit; }

    mbedtls_printf("\r\n Read/write data from/to server:\r\n ");
    fflush( stdout);

    do
    {
    	if(ReadWriteData() < 0) { break; }
    } while(1);

    /* Close session */
    do {
      mbedtls_ssl_close_notify(&ssl);
    } while(ret == MBEDTLS_ERR_SSL_WANT_WRITE);

    ret = mbedtls_ssl_session_reset(&ssl);
    if(ret != 0)
    {
      mbedtls_printf("ERROR - mbedtls_ssl_session_reset returned %d\r\n \r\n ", ret);
      break;
    }

    mbedtls_net_free(&server_fd);
    mbedtls_printf("xPortGetFreeHeapSize: %d\r\n \r\n ", xPortGetFreeHeapSize());
    osDelay(5000);
  }

  exit:
  mbedtls_printf("ERROR on mbedtls...\r\n \r\n ");

  ClearData();

#if defined(MBEDTLS_MEMORY_BUFFER_ALLOC_C)
  mbedtls_memory_buffer_alloc_free();
#endif

  vTaskDelete(NULL);
}
