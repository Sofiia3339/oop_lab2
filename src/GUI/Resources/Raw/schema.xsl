<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes"/>

	<xsl:template match="/">
		<html>
			<head>
				<title>Розклад занять</title>
				<style>
					body { font-family: Arial, sans-serif; margin: 20px; }
					h2 { color: #333; border-bottom: 2px solid #4CAF50; padding-bottom: 10px; }
					table { width: 100%; border-collapse: collapse; margin-top: 20px; }
					th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
					th { background-color: #4CAF50; color: white; }
					tr:nth-child(even) { background-color: #f2f2f2; }
					tr:hover { background-color: #ddd; }
				</style>
			</head>
			<body>
				<h2>Звіт: Розклад Занять</h2>

				<table>
					<tr>
						<th>День</th>
						<th>Час</th>
						<th>Предмет</th>
						<th>Аудиторія</th>
						<th>Викладач</th>
						<th>Групи</th>
					</tr>
					<!-- Ми шукаємо елементи Class, які ми створили в CreateTempXmlFile -->
					<xsl:for-each select="//Class">
						<tr>
							<td>
								<xsl:value-of select="Day"/>
							</td>
							<td>
								<xsl:value-of select="Time"/>
							</td>
							<td>
								<xsl:value-of select="Subject"/>
							</td>
							<td style="font-weight:bold;">
								<xsl:value-of select="Audience"/>
							</td>
							<td>
								<xsl:value-of select="Teacher"/>
							</td>
							<td>
								<xsl:value-of select="Groups"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>

				<xsl:if test="count(//Class) = 0">
					<p style="color: red; padding: 20px;">Дані відсутні або сталася помилка при формуванні звіту.</p>
				</xsl:if>

				<p style="font-size: small; color: gray; margin-top: 20px;">
					Згенеровано автоматично: <script>document.write(new Date().toLocaleString());</script>
				</p>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>